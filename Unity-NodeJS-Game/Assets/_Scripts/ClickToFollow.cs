using UnityEngine;

public class ClickToFollow : MonoBehaviour, IClickable
{
	public GameObject currentPlayer;

    private NetworkEntity networkEntity;
	private Targeter currentPlayerTargeter;

    void Start()
    {
        this.networkEntity = GetComponent<NetworkEntity>();
		this.currentPlayerTargeter = currentPlayer.GetComponent<Targeter> ();
    }

    public void OnClick(RaycastHit hit)
    {
        Debug.Log("follow " + hit.collider.gameObject.name);

        Network.Follow(networkEntity.id);
		this.currentPlayerTargeter.target = transform;

    }
}
