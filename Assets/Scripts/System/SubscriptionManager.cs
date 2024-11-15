using UnityEngine;
using UnityEngine.Purchasing;

namespace Cocktailor
{
    /// <summary>
    /// Mmanages subscriptions and purchases for the application.
    /// </summary>
    public class SubscriptionManager : MonoBehaviour, IStoreListener
    {
        [SerializeField] private GameObject loading;
        [SerializeField] private GameObject[] locks;
        [SerializeField] private GameObject pro, lite;
        [SerializeField] private RecipeViewerPanel main;

        private bool IsInitialized;
        private IStoreController storeController;
        private IExtensionProvider storeExtensionProvider;

        private void Start()
        {
            InitializePurchasing();
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extension)
        {
            if (IsInitialized)
                //구매항목 복원 성공
                PopupMessageManager.Instance.ShowMsg("구매내역을 성공적으로 불러왔습니다.");

            Debug.Log("유니티 IAP 초기화 성공");
            IsInitialized = true;
            storeController = controller;
            storeExtensionProvider = extension;

            var subscribed_month = IsSubscribedTo(PrivateData.productIDSubscription_month);
            var subscribed_year = IsSubscribedTo(PrivateData.productIDSubscription_year);

            if (subscribed_month) PopupMessageManager.Instance.ShowMsg("칵테일러PRO 월간 요금제 구독중입니다!");
            if (subscribed_year) PopupMessageManager.Instance.ShowMsg("칵테일러PRO 연간 요금제 구독중입니다!");

            if (subscribed_month | subscribed_year) PlayerPrefs.SetInt("subscribed", 1);
            else PlayerPrefs.SetInt("subscribed", 0);
            PlayerPrefs.Save();
            UpdateLocks();
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            Debug.LogError($"유니티 IAP 초기화 실패 {error}");
            PopupMessageManager.Instance.ShowMsg("구독 데이터를 초기화하는데 실패했습니다.\n");
            UpdateLocks();
        }

        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {
            Debug.LogError($"유니티 IAP 초기화 실패 {error}");
            PopupMessageManager.Instance.ShowMsg("구독 데이터를 초기화하는데 실패했습니다.\n" + message);
            UpdateLocks();
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
        {
            Debug.Log($"구매 성공 - ID : {args.purchasedProduct.definition.id}");

            if (args.purchasedProduct.definition.id == PrivateData.productIDSubscription_month)
            {
                Debug.Log("월간 구독 서비스 구매 완료");
                PopupMessageManager.Instance.ShowMsg("월간 구독 서비스 구독 완료!");
                PlayerPrefs.SetInt("subscribed", 1);
                PlayerPrefs.Save();
                UpdateLocks();

                var dialog = AlertDialog.Instance.CreateInstance();
                dialog.SetTitle("구독성공");
                dialog.SetMessage("칵테일러Pro를 구독해주셔서 감사합니다!\n이용하시면서 불편한 점이나 추가로 필요한 기능이 있으시면 언제든지 문의해 주세요.");
                dialog.AddButton("확인", () =>
                {
                });
                dialog.Show();
            }
            else if (args.purchasedProduct.definition.id == PrivateData.productIDSubscription_year)
            {
                Debug.Log("연간 구독 서비스 구매 완료");
                PopupMessageManager.Instance.ShowMsg("연간 구독 서비스 구매 완료!");
                PlayerPrefs.SetInt("subscribed", 1);
                PlayerPrefs.Save();
                UpdateLocks();

                var dialog = AlertDialog.Instance.CreateInstance();
                dialog.SetTitle("구독성공");
                dialog.SetMessage("칵테일러Pro를 구독해주셔서 감사합니다!\n이용하시면서 불편한 점이나 추가로 필요한 기능이 있으시면 언제든지 문의해 주세요.");
                dialog.AddButton("확인", () =>
                {
                });
                dialog.Show();
            }
            
            loading.SetActive(false);
            return PurchaseProcessingResult.Complete;
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            loading.SetActive(false);
            Debug.LogWarning($"구매 실패 - {product.definition.id}, {failureReason}");
        }

        public void InitializePurchasing()
        {
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

            builder.AddProduct(PrivateData.productIDSubscription_month, ProductType.Subscription, new IDs
            {
                { PrivateData.Android_SubscriptionId_month, GooglePlay.Name },
                { PrivateData.IOS_SubscriptionId_month, AppleAppStore.Name }
            });

            builder.AddProduct(PrivateData.productIDSubscription_year, ProductType.Subscription, new IDs
            {
                { PrivateData.Android_SubscriptionId_year, GooglePlay.Name },
                { PrivateData.IOS_SubscriptionId_year, AppleAppStore.Name }
            });

            UnityPurchasing.Initialize(this, builder);
        }

        public void UpdateLocks()
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
        }

        private bool IsSubscribedTo(string productId)
        {
            var subscription = storeController.products.WithID(productId);
            if (subscription.receipt == null) return false;

            var subscriptionManager = new UnityEngine.Purchasing.SubscriptionManager(subscription, null);
            var info = subscriptionManager.getSubscriptionInfo();

            return info.isSubscribed() == Result.True;
        }

        public void Purchase(string productId)
        {
            loading.SetActive(true);
            if (!IsInitialized)
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

        public void PurchaseMonth()
        {
            Purchase(PrivateData.productIDSubscription_month);
        }

        public void PurchaseYear()
        {
            Purchase(PrivateData.productIDSubscription_year);
        }
    }
}