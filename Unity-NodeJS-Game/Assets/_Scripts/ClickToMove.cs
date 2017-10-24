using UnityEngine;

public class ClickToMove : MonoBehaviour, IClickable
{
    public GameObject player;

    private Navigator playerNavigator;

    void Start ()
	{
	    this.playerNavigator = player.GetComponent<Navigator>();
	}

    public void OnClick(RaycastHit hit)
    {
        this.playerNavigator.NavigateTo(hit.point);

        Network.Move(player.transform.position, hit.point);
    }
}
