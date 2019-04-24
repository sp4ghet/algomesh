using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*
Quadtree by Just a Pixel (Danny Goodayle) - http://www.justapixel.co.uk
Copyright (c) 2015
Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/
//Any object that you insert into the tree must implement this interface
public interface IQuadTreeObject {
    Vector2 GetPosition();
}
public class QuadTree<T> where T : IQuadTreeObject {
    private int m_maxObjectCount;
    private List<T> m_storedObjects;
    private Rect m_bounds;
    private QuadTree<T>[] cells;

    public QuadTree(int maxSize, Rect bounds) {
        m_bounds = bounds;
        m_maxObjectCount = maxSize;
        cells = new QuadTree<T>[4];
        m_storedObjects = new List<T>(maxSize);
    }
    public void Insert(T objectToInsert) {

        if (cells[0] != null) {
            int iCell = GetCellToInsertObject(objectToInsert.GetPosition());
            if (iCell > -1) {
                cells[iCell].Insert(objectToInsert);
            }
            return;
        }
        m_storedObjects.Add(objectToInsert);
        //Objects exceed the maximum count
        if (m_storedObjects.Count > m_maxObjectCount) {
            //Split the quad into 4 sections
            if (cells[0] == null) {
                float subWidth = m_bounds.width / 2f;
                float subHeight = m_bounds.height / 2f;
                float x = m_bounds.x;
                float y = m_bounds.y;
                cells[0] = new QuadTree<T>(m_maxObjectCount, new Rect(x + subWidth, y, subWidth, subHeight));
                cells[1] = new QuadTree<T>(m_maxObjectCount, new Rect(x, y, subWidth, subHeight));
                cells[2] = new QuadTree<T>(m_maxObjectCount, new Rect(x, y + subHeight, subWidth, subHeight));
                cells[3] = new QuadTree<T>(m_maxObjectCount, new Rect(x + subWidth, y + subHeight, subWidth, subHeight));
            }
            //Reallocate this quads objects into its children
            int i = m_storedObjects.Count - 1; ;
            while (i >= 0) {
                T storedObj = m_storedObjects[i];
                int iCell = GetCellToInsertObject(storedObj.GetPosition());
                if (iCell > -1) {
                    cells[iCell].Insert(storedObj);
                }
                m_storedObjects.RemoveAt(i);
                i--;
            }
        }
    }
    public void Remove(T objectToRemove) {
        if (ContainsLocation(objectToRemove.GetPosition())) {
            m_storedObjects.Remove(objectToRemove);
            if (cells[0] != null) {
                for (int i = 0; i < 4; i++) {
                    cells[i].Remove(objectToRemove);
                }
            }
        }
    }
    public List<T> RetrieveObjectsInArea(Rect area) {
        if (rectOverlap(m_bounds, area)) {
            List<T> returnedObjects = new List<T>();
            for (int i = 0; i < m_storedObjects.Count; i++) {
                if (area.Contains(m_storedObjects[i].GetPosition())) {
                    returnedObjects.Add(m_storedObjects[i]);
                }
            }
            if (cells[0] != null) {
                for (int i = 0; i < 4; i++) {
                    List<T> cellObjects = cells[i].RetrieveObjectsInArea(area);
                    if (cellObjects != null) {
                        returnedObjects.AddRange(cellObjects);
                    }
                }
            }
            return returnedObjects;
        }
        return null;
    }

    // Clear quadtree
    public void Clear() {
        m_storedObjects.Clear();

        for (int i = 0; i < cells.Length; i++) {
            if (cells[i] != null) {
                cells[i].Clear();
                cells[i] = null;
            }
        }
    }
    public bool ContainsLocation(Vector2 location) {
        return m_bounds.Contains(location);
    }
    private int GetCellToInsertObject(Vector2 location) {
        for (int i = 0; i < 4; i++) {
            if (cells[i].ContainsLocation(location)) {
                return i;
            }
        }
        return -1;
    }
    bool valueInRange(float value, float min, float max) { return (value >= min) && (value <= max); }

    bool rectOverlap(Rect A, Rect B) {
        bool xOverlap = valueInRange(A.x, B.x, B.x + B.width) ||
                        valueInRange(B.x, A.x, A.x + A.width);

        bool yOverlap = valueInRange(A.y, B.y, B.y + B.height) ||
                        valueInRange(B.y, A.y, A.y + A.height);

        return xOverlap && yOverlap;
    }
    public void DrawDebug() {
        Gizmos.DrawLine(new Vector3(m_bounds.x, 0, m_bounds.y), new Vector3(m_bounds.x, 0, m_bounds.y + m_bounds.height));
        Gizmos.DrawLine(new Vector3(m_bounds.x, 0, m_bounds.y), new Vector3(m_bounds.x + m_bounds.width, 0, m_bounds.y));
        Gizmos.DrawLine(new Vector3(m_bounds.x + m_bounds.width, 0, m_bounds.y), new Vector3(m_bounds.x + m_bounds.width, 0, m_bounds.y + m_bounds.height));
        Gizmos.DrawLine(new Vector3(m_bounds.x, 0, m_bounds.y + m_bounds.height), new Vector3(m_bounds.x + m_bounds.width, 0, m_bounds.y + m_bounds.height));
        if (cells[0] != null) {
            for (int i = 0; i < cells.Length; i++) {
                if (cells[i] != null) {
                    cells[i].DrawDebug();
                }
            }
        }
    }


    public struct InstancingInfo {
        Vector3 m_pos;
        Vector3 m_size;
        public InstancingInfo(Vector3 pos, Vector3 size) {
            m_pos = pos;
            m_size = size;
        }

        public Vector3 Pos { get => m_pos; set => m_pos = value; }
        public Vector3 Size { get => m_size; set => m_size = value; }
    }

    public (Vector3, Vector3) CreateObject() {
        Vector3 pos = new Vector3(m_bounds.center.x, m_bounds.center.y, 0);
        Vector3 size = new Vector3(m_bounds.width * 0.5f, m_bounds.height * 0.5f, 0.25f*(m_bounds.width + m_bounds.height));

        return (pos, size);
    }

    public void CreateObjects(ref List<(Vector3, Vector3)> infos, int depth=-1) {
        if (cells[0] == null ||  depth == 0) {
            infos.Add(CreateObject());
            return;
        }

        for (int i=0; i < cells.Length; i++) {
            if(cells[i] != null) {
                cells[i].CreateObjects(ref infos, depth-1);
            }   
        }
    }

    private void GenUV(ref Texture2D uv, float shiftX, float shiftY) {
        if (cells[0] == null) {

            float randomId = Random.Range(0f, 1f);
            for (int y = 0; y < Mathf.RoundToInt(m_bounds.height); y++) {
                int yCoord = Mathf.RoundToInt(y + m_bounds.y + shiftY);
                for (int x = 0; x < Mathf.RoundToInt(m_bounds.width); x++) {
                    uv.SetPixel(Mathf.RoundToInt(x + m_bounds.x + shiftX), yCoord, new Color( (x/m_bounds.width) % 1f, (y/m_bounds.height) % 1f, randomId)); 
                }
            }

            //set edges manually
            for (int y = 0; y < Mathf.CeilToInt(m_bounds.height); y++) {
                int yCoord = Mathf.FloorToInt(y + m_bounds.y + shiftY);
                uv.SetPixel(Mathf.FloorToInt(m_bounds.x + shiftX), yCoord, new Color(0f, y / m_bounds.height, randomId));
                uv.SetPixel(Mathf.CeilToInt(m_bounds.x + m_bounds.width + shiftX), yCoord, new Color(1f, y / m_bounds.height, randomId));
            }
            for (int x = 0; x < Mathf.CeilToInt(m_bounds.width); x++) {
                int xCoord = Mathf.FloorToInt(x + m_bounds.x + shiftX);
                uv.SetPixel(xCoord, Mathf.FloorToInt(m_bounds.y + shiftY), new Color(x / m_bounds.width, 0f, randomId));
                uv.SetPixel(xCoord, Mathf.CeilToInt(m_bounds.y + m_bounds.height + shiftY), new Color(x / m_bounds.width, 1f, randomId));
            }

            return;
        }
        

        for (int i = 0; i < cells.Length; i++) {
            if (cells[i] != null) {
                cells[i].GenUV(ref uv, shiftX, shiftY);
            }
        }
    }

    public Texture2D TreeUV() {
        float shiftX = -m_bounds.x;
        float shiftY = -m_bounds.y;
        Texture2D uv = new Texture2D(Mathf.RoundToInt(m_bounds.width), Mathf.RoundToInt(m_bounds.height), TextureFormat.ARGB32, false, true);
        GenUV(ref uv, shiftX, shiftY);
        uv.Apply();
        return uv;
    }
}