using UnityEngine;

public class HexCellShaderData : MonoBehaviour
{
	Texture2D cellTexture;
	Color32[] cellTextureData;

	public void Initialize (int x, int z)
    {
		if (this.cellTexture)
        {
			this.cellTexture.Resize(x, z);
		}
		else
        {
			this.cellTexture = new Texture2D(x, z, TextureFormat.RGBA32, false, true);
			this.cellTexture.filterMode = FilterMode.Point;
			this.cellTexture.wrapMode = TextureWrapMode.Clamp;

            Shader.SetGlobalTexture("_HexCellData", cellTexture);
		}
		Shader.SetGlobalVector("_HexCellData_TexelSize", new Vector4(1f / x, 1f / z, x, z));

		if (this.cellTextureData == null || this.cellTextureData.Length != x * z)
        {
			this.cellTextureData = new Color32[x * z];
		}
		else
        {
			for (int i = 0; i < this.cellTextureData.Length; i++)
            {
				this.cellTextureData[i] = new Color32(0, 0, 0, 0);
			}
		}

		enabled = true;
	}

	public void RefreshTerrain (HexCell cell)
    {
		this.cellTextureData[cell.Index].a = (byte)cell.TerrainTypeIndex;
		enabled = true;
	}

	public void RefreshVisibility (HexCell cell)
    {
		this.cellTextureData[cell.Index].r = cell.IsVisible ? (byte)255 : (byte)0;
		enabled = true;
	}

	void LateUpdate ()
    {
		this.cellTexture.SetPixels32(cellTextureData);
		this.cellTexture.Apply();

		enabled = false;
	}
}