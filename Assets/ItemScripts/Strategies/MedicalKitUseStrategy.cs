using System.Collections;
using UnityEngine;

public class MedicalKitUseStrategy : IUseStrategy
{
    public bool Use(Player player, Consumable consumable)
    {
        if (consumable is MedicalKit medicalKit)
        {
            if (player.currentHp < player.maxHp)
            {
                medicalKit.UseAnimation();
                player.StartCoroutine(UseMedicalKitCoroutine(player, medicalKit));
                return true;
            }
            else
            {
                MessagePopup.Show("Player health is already full.");
                return false;
            }
        }
        else
        {
            Debug.LogError("Consumable is not a MedicalKit.");
            return false;
        }
    }
    
    private IEnumerator UseMedicalKitCoroutine(Player player, MedicalKit medicalKit)
    {
        yield return new WaitForSeconds(medicalKit.useTime);
        player.ChangeHealthDelta(medicalKit.hp);
        GameObject.Destroy(medicalKit.gameObject);
    }
}