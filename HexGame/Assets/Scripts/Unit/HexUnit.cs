using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class HexUnit : MonoBehaviour
{
	const float ROTATION_SPEED = 180f;
	const float TRAVEL_SPEED = 4f;
    const int   VISION_RANGE = 3;

	public static HexUnit unitPrefab;

	public HexGrid Grid { get; set; }

    HexCell location;
	public HexCell Location
    {
		get
        {
			return this.location;
		}
		set
        {
			if (this.location)
            {
				Grid.DecreaseVisibility(this.location, VISION_RANGE);
				this.location.Unit = null;
			}

			this.location = value;
			value.Unit = this;
			Grid.IncreaseVisibility(value, VISION_RANGE);
			this.transform.localPosition = value.Position;
		}
	}

	float orientation;
	public float Orientation {
		get {
			return orientation;
		}
		set {
			orientation = value;
			this.transform.localRotation = Quaternion.Euler(0f, value, 0f);
		}
	}

    HexCell currentTravelLocation;
	List<HexCell> pathToTravel;

	public void ValidateLocation ()
    {
		this.transform.localPosition = location.Position;
	}

	public bool IsValidDestination (HexCell cell)
    {
		return !cell.IsUnderwater && !cell.Unit;
	}

	public void Travel (List<HexCell> path)
    {
        InitPath(path);

        StopAllCoroutines();
        StartCoroutine(TravelPath());
    }

	public void Die ()
    {
		if (this.location)
        {
			Grid.DecreaseVisibility(this.location, VISION_RANGE);
		}

        this.location.Unit = null;
		Destroy(gameObject);
	}

    private void InitPath(List<HexCell> path)
    {
        this.location.Unit = null;
        {
            this.location = path[path.Count - 1];
            this.location.Unit = this;
        }

        this.pathToTravel = path;
    }

    IEnumerator TravelPath ()
    {
		Vector3 a, b, c = this.pathToTravel[0].Position;

		yield return LookAt(this.pathToTravel[1].Position);

		Grid.DecreaseVisibility(
            this.currentTravelLocation ? this.currentTravelLocation : this.pathToTravel[0], VISION_RANGE);

		float timeToTravelSpeed = Time.deltaTime * TRAVEL_SPEED;
		for (int i = 1; i < this.pathToTravel.Count; i++)
        {
			this.currentTravelLocation = pathToTravel[i];
			a = c;
			b = this.pathToTravel[i - 1].Position;
			c = (b + this.currentTravelLocation.Position) * 0.5f;

            Grid.IncreaseVisibility(pathToTravel[i], VISION_RANGE);

            for (; timeToTravelSpeed < 1f; timeToTravelSpeed += Time.deltaTime * TRAVEL_SPEED)
            {
				this.transform.localPosition = Bezier.GetPoint(a, b, c, timeToTravelSpeed);

                Vector3 d = Bezier.GetDerivative(a, b, c, timeToTravelSpeed);
				d.y = 0f;
				this.transform.localRotation = Quaternion.LookRotation(d);

                yield return null;
			}
			Grid.DecreaseVisibility(pathToTravel[i], VISION_RANGE);
			timeToTravelSpeed -= 1f;
		}

        this.currentTravelLocation = null;

		a = c;
		b = location.Position;
		c = b;

		Grid.IncreaseVisibility(location, VISION_RANGE);

        for (; timeToTravelSpeed < 1f; timeToTravelSpeed += Time.deltaTime * TRAVEL_SPEED)
        {
			this.transform.localPosition = Bezier.GetPoint(a, b, c, timeToTravelSpeed);

            Vector3 d = Bezier.GetDerivative(a, b, c, timeToTravelSpeed);
			d.y = 0f;
			this.transform.localRotation = Quaternion.LookRotation(d);

            yield return null;
		}

		this.transform.localPosition = this.location.Position;
		this.orientation = this.transform.localRotation.eulerAngles.y;

        ListPool<HexCell>.Add(pathToTravel);

        pathToTravel = null;
	}

	IEnumerator LookAt (Vector3 targetPoing)
    {
		targetPoing.y = transform.localPosition.y;
		Quaternion fromRotation = transform.localRotation;
		Quaternion toRotation =
			Quaternion.LookRotation(targetPoing - transform.localPosition);

        float angle = Quaternion.Angle(fromRotation, toRotation);
		if (angle > 0f)
        {
			float speed = ROTATION_SPEED / angle;

            for (float t = Time.deltaTime * speed; t < 1f; t += Time.deltaTime * speed)
            {
				this.transform.localRotation =
					Quaternion.Slerp(fromRotation, toRotation, t);

                yield return null;
			}
		}

		this.transform.LookAt(targetPoing);
		this.orientation = transform.localRotation.eulerAngles.y;
	}

    public void Save(BinaryWriter writer)
    {
        this.location.coordinates.Save(writer);

        writer.Write(orientation);
    }

    // TODO : UnitManager로 이동
    public static void Load (BinaryReader reader, HexGrid grid)
    {
		HexCoordinates coordinates = HexCoordinates.Load(reader);
		float orientation = reader.ReadSingle();

        grid.AddUnit(Instantiate(unitPrefab), grid.GetCell(coordinates), orientation);
	}

	void OnEnable ()
    {
		if (this.location)
        {
			this.transform.localPosition = this.location.Position;

            if (this.currentTravelLocation)
            {
				Grid.IncreaseVisibility(this.location, VISION_RANGE);
				Grid.DecreaseVisibility(this.currentTravelLocation, VISION_RANGE);

                this.currentTravelLocation = null;
			}
		}
	}

    // PathDraw Debug
//	void OnDrawGizmos () {
//		if (pathToTravel == null || pathToTravel.Count == 0) {
//			return;
//		}
//
//		Vector3 a, b, c = pathToTravel[0].Position;
//
//		for (int i = 1; i < pathToTravel.Count; i++) {
//			a = c;
//			b = pathToTravel[i - 1].Position;
//			c = (b + pathToTravel[i].Position) * 0.5f;
//			for (float t = 0f; t < 1f; t += 0.1f) {
//				Gizmos.DrawSphere(Bezier.GetPoint(a, b, c, t), 2f);
//			}
//		}
//
//		a = c;
//		b = pathToTravel[pathToTravel.Count - 1].Position;
//		c = b;
//		for (float t = 0f; t < 1f; t += 0.1f) {
//			Gizmos.DrawSphere(Bezier.GetPoint(a, b, c, t), 2f);
//		}
//	}
}