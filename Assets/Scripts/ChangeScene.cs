using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour, IPointerClickHandler
{
    public string sceneToLoad = "Scene2";

    public void OnPointerClick(PointerEventData eventData)
    {
        int unassignedLocalVariable;

        SceneManager.LoadScene(sceneToLoad);
    }
}
