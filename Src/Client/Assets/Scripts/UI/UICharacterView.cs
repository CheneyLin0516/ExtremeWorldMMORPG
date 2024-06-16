using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICharacterView : MonoBehaviour {

    public GameObject[] characters;

    private int currentCharacter = 0;

    public int CurrentCharacter
    {
        get
        {
            return currentCharacter;
        }
        set
        {
            currentCharacter = value;
            this.UpdateCharacter();
        }
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void UpdateCharacter()
    {
        for( int i = 0; i < 3; i++)
        {
            characters[i].SetActive(i == this.currentCharacter);//如果 i 等于 currentCharacter，表达式结果为 true；否则为 false。
        }
    }
    //在角色选择视图中，我们不仅需要显示当前选中的角色，还需要确保其他角色被隐藏。
    //虽然点击选择一个角色，但为了更新所有角色的显示状态，仍然需要遍历所有角色。
}
