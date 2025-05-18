using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;
public class WeaponChanger : MonoBehaviour
{
    [SerializeField]
    MultiParentConstraint weaponParent;
    [SerializeField]
    int lerp;

    private Animator playerAnim;
    public bool equip = false;
    private void Start()
    {
        playerAnim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            playerAnim.SetTrigger("gunPick");
            playerAnim.SetBool("isGunActive",!equip);
            equip = !equip;
        }
    }
    public void EquipAndDisable()
    {
        StartCoroutine(ChangeWeapon(equip));
    }

    IEnumerator ChangeWeapon(bool equip)
    {

        var sources = weaponParent.data.sourceObjects;
        for (int i = 0; i <= lerp; i++)
        {
            yield return new WaitForFixedUpdate();
            float toZero = Mathf.Lerp(1, 0, i / lerp);
            float toOne = Mathf.Lerp(0, 1, i / lerp);
            sources.SetWeight(0, equip ? toZero : toOne);
            sources.SetWeight(1, equip ? toOne : toZero);
        }


        weaponParent.data.sourceObjects = sources;
    }
}
