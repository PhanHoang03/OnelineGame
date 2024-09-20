    using UnityEngine;
    using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class DragLineCtrl : MonoBehaviour
    {
        private List<LineRenderer> lineRenderer = new List<LineRenderer>();
        [SerializeField] protected List<Transform> linePoints = new List<Transform>();
        [SerializeField] protected LineRenderer line;
        [SerializeField] protected LevelSO levelSO;
        [SerializeField] protected PathCtrl pathCtrl;
        [SerializeField] protected UICtrl uICtrl;
        private EventSystem eventSystem;
        private Transform prevDot;
        private Transform curDot;
        private int numLine = 0;
        private int lineIndex = -1;

        // LayerMask for detecting dots (optional, if you need to isolate the dot objects)
        public LayerMask dotLayerMask;

        private void Awake()
        {
            this.SetUp();
        }

        private void SetUp()
        {
            this.LoadEventSystem();
            this.ClearLine();
        }

        private void LoadEventSystem()
        {
            GameObject source = GameObject.Find("EventSystem");
            this.eventSystem = source.GetComponent<EventSystem>();
        }

        private void Update()
        {
            this.HandleLineDrawing();
        }

        private bool IsPointerOverUIObject()
        {
            PointerEventData eventData = new PointerEventData(eventSystem);
            eventData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            eventSystem.RaycastAll(eventData, results);
            return results.Count > 0;
        }

        private void HandleLineDrawing()
        {

            if (this.IsPointerOverUIObject())
            {
                // Don't proceed with line drawing logic if the click is on a UI element
                return;
            }
            
            // Mouse or touch position
            Vector3 mousePos = Input.mousePosition;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 0f));

            // If the mouse button is pressed or a touch begins
            if (Input.GetMouseButtonDown(0))
            {
                // Check if a dot is clicked
                RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero, Mathf.Infinity, dotLayerMask);

                if (hit.collider != null)  
                {
                    Debug.Log(hit.collider.transform.name);
                    this.AddPointToLine(hit.collider.transform);
                }
            }

            // When dragging, continue the line
            if (Input.GetMouseButton(0) && this.linePoints.Count > 0)
            {
                RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero, Mathf.Infinity, dotLayerMask);
                if (hit.collider != null)
                {
                    Vector3 hitPosition = hit.collider.transform.position;

                    if (this.linePoints[this.linePoints.Count - 1].position != hitPosition)
                    {
                        Debug.Log(hit.collider.transform.name);
                        this.prevDot = this.linePoints[this.linePoints.Count - 1];

                        this.AddPointToLine(hit.collider.transform);
                    }
                }
                else
                {
                    this.lineRenderer[this.lineIndex].SetPosition(1, worldPos);
                }
            }

            // If mouse button is released, finish the line
            if (Input.GetMouseButtonUp(0))
            {
                int index = this.linePoints.Count - 1;
                if (index >= 0) this.lineRenderer[this.lineIndex].SetPosition(1, linePoints[index].position);
            }
        }

        // Add a new point to the line
        private void AddPointToLine(Transform newPoint)
        {
            if (this.linePoints.Count > 0)
            {
                int index1 = this.DotIndex(this.prevDot.name);
                int index2 = this.DotIndex(newPoint.name);

                //if (index1 == index2) return;
                //if (index1 > index2) (index1, index2) = (index2, index1);
                if (this.levelSO.board[index1]._row[index2] == false) return;
                if (this.levelSO.checkBoard[index1]._row[index2] == true) return;
                this.levelSO.checkBoard[index1]._row[index2] = true;
                this.levelSO.checkBoard[index2]._row[index1] = true;
                this.numLine++;
            }

            if (this.CheckClear()) 
            {
                this.uICtrl.GameClearUI();
            }

            if (this.prevDot != null) this.prevDot.localScale = new Vector3 (0.8f, 0.8f, 0f);

            newPoint.localScale = new Vector3 (1.1f, 1.1f, 0f);

            LineRenderer lr = Instantiate(line);
            lr.transform.parent = transform;
            this.linePoints.Add(newPoint);
            if(this.lineRenderer.Count < this.numLine + 1) this.lineRenderer.Add(lr);
            this.curDot = newPoint;
            this.lineIndex++;
            int index = this.lineIndex;
            this.lineRenderer[index].gameObject.SetActive(true);
            this.lineRenderer[index].positionCount = 2;
            this.lineRenderer[index].SetPosition(0, newPoint.position);
            this.lineRenderer[index].SetPosition(1, newPoint.position);
            if (index > 0) this.lineRenderer[index - 1].SetPosition(1, newPoint.position);
        }

        private int DotIndex(string dotName)
        {
            bool check = false;
            int index = 0;
            for (int i = 0; i < dotName.Length; i++)
            {
                if (dotName[i] == '_') 
                {
                    check = true;
                    continue;
                }

                if (check == true) 
                {
                    index = index * 10 + dotName[i] - '0';
                }
            }
            return index;
        }

        public bool CheckClear()
        {
            if (numLine == this.pathCtrl.NumPath) return true;
            return false;
        }

        public void TraceLine()
        {
            if (this.numLine == 0) 
            {
                this.ClearLine();
                return;
            }

            this.curDot.localScale = new Vector3(0.8f, 0.8f, 0f);

            this.linePoints.RemoveAt(linePoints.Count - 1);
            Transform cDot = this.linePoints[linePoints.Count - 1]; 

            int index1 = DotIndex(cDot.name);
            int index2 = DotIndex(curDot.name);

            if (lineRenderer[this.lineIndex].GetPosition(0) == lineRenderer[this.lineIndex].GetPosition(1))
            {
                this.lineRenderer[this.lineIndex].gameObject.SetActive(false);
                this.lineIndex--;
                this.numLine--;
            }
            
            this.lineRenderer[this.lineIndex].gameObject.SetActive(false);
            this.lineIndex--;
            this.numLine--;

            this.curDot = cDot; 
            cDot.localScale = new Vector3(1.1f, 1.1f, 0f);

            this.levelSO.checkBoard[index1]._row[index2] = false;
            this.levelSO.checkBoard[index2]._row[index1] = false;
        }

        public void ClearLine()
        {
            this.numLine = 0;
            this.lineIndex = -1;
            if (this.curDot != null) this.curDot.localScale = new Vector3(0.8f, 0.8f, 0f);
            this.prevDot = null;
            this.curDot = null;
            for (int i = 0; i < lineRenderer.Count - 1; i++)
            {
                lineRenderer[i].gameObject.SetActive(false);
            }
            this.linePoints.Clear();
            this.levelSO.CreateCheckBoard();
        }
    }
