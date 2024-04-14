using UnityEngine;
using UnityEngine.UI;
using VoxelBusters.EssentialKit;

namespace Cocktailor
{
    /// <summary>
    /// Manages the display and functionality of various dialogs in the application.
    /// </summary>
    public class DialogManager
    {
        public void ShowResolutionChangeDialog(Dropdown resolutionSet)
        {
            var dialog = AlertDialog.CreateInstance();
            dialog.Title = "경고";
            dialog.Message = "바뀐 해상도 설정을 유지할까요?";

            dialog.AddButton("네", () => { PlayerPrefs.SetInt("resolutionSet", resolutionSet.value); });
            dialog.AddCancelButton("아니요", () =>
            {
                resolutionSet.value = PlayerPrefs.GetInt("resolutionSet");
                SettingsManager.Instance?.ChangeResolution();
            });
            dialog.Show(); //Show the dialog
            SfxManager.Instance?.PlaySfx(7);
        }

        public void ShowDataResetDialog()
        {
            var dialog = AlertDialog.CreateInstance();
            dialog.Title = "경고";
            dialog.Message = "데이터를 초기화하고 앱을 재실행할까요?";

            dialog.AddButton("네", () =>
            {
                PlayerPrefs.DeleteAll();
                Application.Quit();
            });
            dialog.AddCancelButton("아니요", () => { });
            dialog.Show();
            SfxManager.Instance?.PlaySfx(7);
        }
    }
}