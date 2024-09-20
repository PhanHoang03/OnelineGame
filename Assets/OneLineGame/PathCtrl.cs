using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class PathCtrl : MonoBehaviour
{
    private List<LineRenderer> lineRenderer = new List<LineRenderer>();
    [SerializeField] protected GameObject arrowPrefab;
    [SerializeField] protected List<GameObject> dots = new List<GameObject>();
    [SerializeField] protected LevelSO levelSO;
    [SerializeField] protected LineRenderer linePreset;
    protected int numPath;
    public int NumPath => numPath;

    private void Start()
    {
        this.SetUpLinePos();
    }

    protected virtual void SetUpLinePos()
    {
        this.arrowPrefab = Resources.Load<GameObject>("Prefabs/Triangle");
        Debug.Log(this.arrowPrefab.name);
        this.lineRenderer.Clear();
        this.numPath = 0;
        for (int i = 0; i < levelSO.numRow; i++) 
        {
            for (int j = i + 1; j < levelSO.numCol; j++)
            {
                if (levelSO.board[i]._row[j] == false && levelSO.board[j]._row[i] == false) continue;
                this.numPath++;
                LineRenderer lr = Instantiate(linePreset);
                lr.gameObject.SetActive(true);
                lr.transform.parent = transform;    
                this.lineRenderer.Add(lr);
                int index = this.lineRenderer.Count - 1;
                this.lineRenderer[index].positionCount = 2;
                this.lineRenderer[index].SetPosition(0, dots[i].transform.position);
                this.lineRenderer[index].SetPosition(1, dots[j].transform.position);

                if (levelSO.board[i]._row[j] == true && levelSO.board[j]._row[i] == true) continue;

                Vector3 startPos = dots[i].transform.position;
                Vector3 endPos = dots[j].transform.position;
                
                if (levelSO.board[i]._row[j] == false) (startPos, endPos) = (endPos, startPos);

                // Calculate the midpoint
                Vector3 midpoint = (startPos + endPos) / 2;

                // Instantiate the arrow prefab at the midpoint
                GameObject arrow = Instantiate(arrowPrefab, midpoint, Quaternion.identity);
                arrow.transform.parent = transform;

                // Rotate the arrow to align with the line direction
                Vector3 direction = (endPos - startPos).normalized;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                arrow.transform.rotation = Quaternion.Euler(0, 0, angle - 90);
            }
        }
    }
}
