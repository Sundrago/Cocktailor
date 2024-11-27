using System;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using Sirenix.OdinInspector;

namespace Cocktailor
{
    /// <summary>
    /// Mmanages subscriptions and purchases for the application.
    /// </summary>
    public class SubscriptionManager : MonoBehaviour, IStoreListener
    {
        public static SubscriptionManager Instance;
        
        [SerializeField] private GameObject loading;
        [SerializeField] private GameObject[] locks;
        [SerializeField] private GameObject pro, lite;
        [SerializeField] private Text[] buttonText;
        [SerializeField] private RecipeViewerPanel main;

        private bool isInitialized;
        private IStoreController storeController;
        private IExtensionProvider storeExtensionProvider;
        public static ItemType CurrentItemType = ItemType.Null;
        public string environment = "production";

        private void Awake()
        {
            Instance = this;
        }
        

        async void Start()
        {
            try
            {
                var options = new InitializationOptions()
                    .SetEnvironmentName(environment);
                await UnityServices.InitializeAsync(options);
                InitializePurchasing();
            }
            catch (Exception exception)
            {
            }
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extension)
        {
            if (isInitialized)
                PopupMessageManager.Instance.ShowMsg("구매내역을 성공적으로 불러왔습니다.");

            Debug.Log("유니티 IAP 초기화 성공");
            isInitialized = true;
            storeController = controller;
            storeExtensionProvider = extension;

            foreach (var productData in PrivateData.ProductDatas)
            {
                if (HasReceipt(productData.ItemType.ToString()))
                {
                    CurrentItemType = productData.ItemType;
                }
            }

            switch (CurrentItemType)
            {
                case ItemType.Null :
                    PlayerPrefs.SetInt("subscribed", 0);
                    break;
                default:
                    PopupMessageManager.Instance.ShowMsg("칵테일러PRO를 이용중입니다!");
                    PlayerPrefs.SetInt("subscribed", 1);
                    break;
            }
            PlayerPrefs.Save();
            UpdateUI();
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            Debug.LogError($"유니티 IAP 초기화 실패 {error}");
            PopupMessageManager.Instance.ShowMsg("구독 데이터를 초기화하는데 실패했습니다.\n");
            UpdateUI();
        }

        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {
            Debug.LogError($"유니티 IAP 초기화 실패 {error}");
            PopupMessageManager.Instance.ShowMsg("구독 데이터를 초기화하는데 실패했습니다.\n" + message);
            UpdateUI();
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
        {
            Debug.Log($"구매 성공 - ID : {args.purchasedProduct.definition.id}");

            if (Enum.TryParse(args.purchasedProduct.definition.id, out CurrentItemType))
            {
                AlertDialogueObject dialog;
                switch (CurrentItemType)
                {
                        
                    case ItemType.Pro:
                        dialog = AlertDialog.Instance.CreateInstance();
                        dialog.SetTitle("구매성공");
                        dialog.SetMessage("칵테일러Pro를 구매해주셔서 감사합니다!\n이용하시면서 불편한 점이나 추가로 필요한 기능이 있으시면 언제든지 문의해 주세요.");
                        dialog.AddButton("확인", () =>
                        {
                        });
                        dialog.Show();
                        PlayerPrefs.SetInt("subscribed", 1);
                        break;
                    case ItemType.Null:
                        PlayerPrefs.SetInt("subscribed", 0);
                        break;
                    default:
                        dialog = AlertDialog.Instance.CreateInstance();
                        dialog.SetTitle("구독성공");
                        dialog.SetMessage("칵테일러Pro를 구독해주셔서 감사합니다!\n이용하시면서 불편한 점이나 추가로 필요한 기능이 있으시면 언제든지 문의해 주세요.");
                        dialog.AddButton("확인", () =>
                        {
                        });
                        dialog.Show();
                        PlayerPrefs.SetInt("subscribed", 1);
                        break;
                }
            }
            else
            {
                CurrentItemType = ItemType.Null;
                PlayerPrefs.SetInt("subscribed", 0);
            }
            
            PlayerPrefs.Save();
            UpdateUI();
            loading.SetActive(false);
            return PurchaseProcessingResult.Complete;
        }

        [Button]
        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            loading.SetActive(false);
            PopupMessageManager.Instance.ShowMsg("구매실패");
            // Debug.LogWarning($"구매 실패 - {product.definition.id}, {failureReason}");
        }

        public void InitializePurchasing()
        {
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

            foreach (var productData in PrivateData.ProductDatas)
            {
                builder.AddProduct(productData.ItemType.ToString(), productData.ProductType, new IDs
                {
                    { productData.ID_AOS, GooglePlay.Name },
                    { productData.ID_IOS, AppleAppStore.Name }
                });
            }

            UnityPurchasing.Initialize(this, builder);
        }

        public void UpdateUI()
        {
            foreach (var obj in locks)
                if (PlayerPrefs.GetInt("subscribed") == 0) obj.SetActive(true);
                else obj.SetActive(false);

            if (PlayerPrefs.GetInt("subscribed") == 0)
            {
                lite.SetActive(true);
                pro.SetActive(false);
            }
            else
            {
                lite.SetActive(false);
                pro.SetActive(true);
            }
            
            buttonText[0].text = storeController.products.WithID(ItemType.Weekly.ToString()).metadata.localizedPriceString + "/주";
            buttonText[1].text = storeController.products.WithID(ItemType.Monthly.ToString()).metadata.localizedPriceString + "/월";
            int discountRate = 100-Mathf.RoundToInt(
                (float)(storeController.products.WithID(ItemType.Weekly.ToString()).metadata.localizedPrice * 4 /
                        storeController.products.WithID(ItemType.Monthly.ToString()).metadata.localizedPrice) * 100f); 
            buttonText[2].text = "(첫 3일 무료 체험. 약 "+discountRate+"%할인)";
            buttonText[3].text = storeController.products.WithID(ItemType.Pro.ToString()).metadata.localizedPriceString + "/무기한";
        }

        private bool HasReceipt(string productId)
        {
            var subscription = storeController.products.WithID(productId);
            if (subscription.receipt == null) return false;
            if (productId == ItemType.Pro.ToString()) return true;

            var subscriptionManager = new UnityEngine.Purchasing.SubscriptionManager(subscription, null);
            var info = subscriptionManager.getSubscriptionInfo();
            return info.isSubscribed() == Result.True;
        }

        public string GetReceipt()
        {
            var subscription = storeController.products.WithID(CurrentItemType.ToString());
            if (subscription.receipt == null) return "";
            return subscription.receipt;
        }
 
        public void Purchase(string productId)
        {
            loading.SetActive(true);
            if (!isInitialized)
            {
                InitializePurchasing();
                return;
            }

            var product = storeController.products.WithID(productId);
            if (product != null && product.availableToPurchase)
            {
                Debug.Log($"구매 시도 - {product.definition.id}");
                storeController.InitiatePurchase(product);
            }
            else
            {
                Debug.Log($"구매 시도 불가 - {productId}");
                loading.SetActive(false);
            }
        }
    }
}