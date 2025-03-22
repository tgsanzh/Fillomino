using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEditor.Progress;

[Serializable]  
public class InnerList
{
    public List<GameObject> elements;
}
public class Controller : MonoBehaviour
{
    public List<InnerList> nestedLists;
    private GameObject chosenObject;
    [SerializeField] private GameObject splitter;
    [SerializeField] private GameObject arena;
    [SerializeField] private GameObject corner;
    public List<GameObject> backObjects = new List<GameObject>();
    public string[] tags = new string[] { "Dark Blue", "Blue", "Red", "Yellow", "Green" };
    public int arenaCount;

    [SerializeField] private GameObject[] blockText;
    [SerializeField] private GameObject[] blocks;
    [SerializeField] private Color[] colors;

    [SerializeField] private GameObject hintButton;
    [SerializeField] private GameObject nextButton;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip winClip;
    [SerializeField] private AudioClip clickClip;

    [SerializeField] private Text levelText;

    private bool win = false;

    public int colorCount = 0;



    private float _startAspect = 1179f / 2556f;

    private float _defaultHeight;
    private float _defaultWidth;
    private void Awake()
    {
        //Camera.main.orthographicSize = 1.2125f * movement.pixels;
        _defaultHeight = Camera.main.orthographicSize;
        _defaultWidth = Camera.main.orthographicSize * _startAspect;

        Camera.main.orthographicSize = _defaultWidth / Camera.main.aspect;
    }

    void Start()
    {
        SystemLanguage sl = Application.systemLanguage;

        if(sl == SystemLanguage.Russian)
        {
            levelText.text = "УРОВЕНЬ " + PlayerPrefs.GetInt("Level", 1);
        }
        else if (sl == SystemLanguage.English)
        {
            levelText.text = "LEVEL " + PlayerPrefs.GetInt("Level", 1);
        }
        else if (sl == SystemLanguage.Spanish)
        {
            levelText.text = "NIVEL " + PlayerPrefs.GetInt("Level", 1);
        }
        else if (sl == SystemLanguage.Hindi)
        {
            levelText.text = "स्तर " + PlayerPrefs.GetInt("Level", 1);
        }
        else if (sl == SystemLanguage.Arabic)
        {
            levelText.text = "مستوى " + PlayerPrefs.GetInt("Level", 1);
        }
        else if (sl == SystemLanguage.Arabic)
        {
            levelText.text = "مستوى " + PlayerPrefs.GetInt("Level", 1);
        }


        //PlayerPrefs.DeleteKey("Level");


        int currentItem = 0;
        foreach (var item in blockText)
        {
            item.GetComponent<Text>().color = Color.white;
            if(item.GetComponent<Text>().text != "")
            {
                item.transform.parent.parent.GetComponent<SpriteRenderer>().color = colors[currentItem];
                item.transform.parent.parent.tag = tags[currentItem];
                currentItem++;
            }
        }
        colorCount = currentItem;
    }

    void Update()
    {
        if(win)
        {
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit)
            {
                if(hit.collider.tag != "White")
                {
                    chosenObject = hit.collider.gameObject;
                }
            }

        }
        if (Input.GetMouseButton(0) && chosenObject != null)
        {

            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit && hit.collider.gameObject != chosenObject)
            {
                if (hit.collider.tag != chosenObject.tag && hit.transform.GetChild(0).GetChild(0).GetComponent<Text>().text == "")
                {
                    RaycastHit2D hitUp = Physics2D.Raycast(new Vector2(hit.transform.position.x, hit.transform.position.y + 1), Vector2.zero);
                    if (hitUp && hitUp.collider.tag == chosenObject.tag)
                    {
                        hit.collider.gameObject.GetComponent<SpriteRenderer>().color = chosenObject.GetComponent<SpriteRenderer>().color;
                        hit.collider.tag = chosenObject.tag;
                        CreateBorder();
                        CheckToWin();
                    }


                    RaycastHit2D hitRight = Physics2D.Raycast(new Vector2(hit.transform.position.x + 1, hit.transform.position.y), Vector2.zero);
                    if (hitRight && hitRight.collider.tag == chosenObject.tag)
                    {
                        hit.collider.gameObject.GetComponent<SpriteRenderer>().color = chosenObject.GetComponent<SpriteRenderer>().color;
                        hit.collider.tag = chosenObject.tag;
                        CreateBorder();
                        CheckToWin();
                    }


                    RaycastHit2D hitDown = Physics2D.Raycast(new Vector2(hit.transform.position.x, hit.transform.position.y - 1), Vector2.zero);
                    if (hitDown && hitDown.collider.tag == chosenObject.tag)
                    {
                        hit.collider.gameObject.GetComponent<SpriteRenderer>().color = chosenObject.GetComponent<SpriteRenderer>().color;
                        hit.collider.tag = chosenObject.tag;
                        CreateBorder();
                        CheckToWin();
                    }


                    RaycastHit2D hitLeft = Physics2D.Raycast(new Vector2(hit.transform.position.x - 1, hit.transform.position.y), Vector2.zero);
                    if (hitLeft && hitLeft.collider.tag == chosenObject.tag)
                    {
                        hit.collider.gameObject.GetComponent<SpriteRenderer>().color = chosenObject.GetComponent<SpriteRenderer>().color;
                        hit.collider.tag = chosenObject.tag;
                        CreateBorder();
                        CheckToWin();
                    }

                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            chosenObject = null;
        }

    }


    void CreateBorder()
    {

        for (int i = 0; i < backObjects.Count; i++)
        {
            Destroy(backObjects[i].gameObject);
        }
        backObjects.Clear();

        for (int i = 0; i < arenaCount; i++)
        {
            GameObject current = arena.transform.GetChild(i).gameObject;

            if (current.tag == "White")
            { 
                continue;
            }

            RaycastHit2D hitRight = Physics2D.Raycast(new Vector2(current.transform.position.x + 1, current.transform.position.y), Vector2.zero);
            RaycastHit2D hitDown = Physics2D.Raycast(new Vector2(current.transform.position.x, current.transform.position.y - 1), Vector2.zero);
            RaycastHit2D hitRightDown = Physics2D.Raycast(new Vector2(current.transform.position.x + 1, current.transform.position.y - 1), Vector2.zero);

            if(hitRight&& hitRight.collider.tag == current.tag)
            {
                GameObject split = Instantiate(splitter, new Vector2(current.transform.position.x + 0.5f, current.transform.position.y), Quaternion.identity);
                split.GetComponent<SpriteRenderer>().color = current.GetComponent<SpriteRenderer>().color;
                backObjects.Add(split);
            }
            if (hitDown && hitDown.collider.tag == current.tag)
            {
                GameObject split = Instantiate(splitter, new Vector2(current.transform.position.x, current.transform.position.y - 0.5f), Quaternion.identity);
                split.GetComponent<SpriteRenderer>().color = current.GetComponent<SpriteRenderer>().color;
                backObjects.Add(split);
            }
            




            if (hitRight && hitDown)
            {
                if(hitRight.collider.tag == current.tag && hitDown.collider.tag == current.tag && hitRightDown.collider.tag != current.tag)
                {
                    GameObject currentCorner = Instantiate(corner, new Vector2(0f, 0f), Quaternion.Euler(0, 0, 180));
                    currentCorner.transform.parent = current.transform;
                    currentCorner.GetComponent<SpriteRenderer>().color = current.GetComponent<SpriteRenderer>().color;
                    currentCorner.transform.localPosition = new Vector2(1.007f, -1.017f);
                    backObjects.Add(currentCorner);
                }

                if (hitRight.collider.tag != current.tag && hitDown.collider.tag == current.tag && hitRightDown.collider.tag == current.tag)
                {
                    GameObject currentCorner = Instantiate(corner, new Vector2(0f, 0f), Quaternion.Euler(0, 0, 270));
                    currentCorner.transform.parent = current.transform;
                    currentCorner.GetComponent<SpriteRenderer>().color = current.GetComponent<SpriteRenderer>().color;
                    currentCorner.transform.localPosition = new Vector2(1.014f, -0.043f);
                    backObjects.Add(currentCorner);
                }

                if (hitRight.collider.tag == current.tag && hitDown.collider.tag != current.tag && hitRightDown.collider.tag == current.tag)
                {
                    GameObject currentCorner = Instantiate(corner, new Vector2(0f, 0f), Quaternion.Euler(0, 0, 90));
                    currentCorner.transform.parent = current.transform;
                    currentCorner.GetComponent<SpriteRenderer>().color = current.GetComponent<SpriteRenderer>().color;
                    currentCorner.transform.localPosition = new Vector2(0.043f, -1.014f);
                    backObjects.Add(currentCorner);
                }

                if (hitRight.collider.tag != current.tag && hitDown.collider.tag != current.tag && hitRightDown.collider.tag != current.tag)
                {
                    if (hitRight.collider.tag != "White" && hitRight.collider.tag == hitDown.collider.tag && hitRight.collider.tag == hitRightDown.collider.tag)
                    {
                        GameObject currentCorner = Instantiate(corner, new Vector2(0f, 0f), Quaternion.Euler(0, 0, 0));
                        currentCorner.transform.parent = current.transform;
                        currentCorner.GetComponent<SpriteRenderer>().color = hitRight.collider.GetComponent<SpriteRenderer>().color;
                        currentCorner.transform.localPosition = new Vector2(0.037f, -0.043f);
                        backObjects.Add(currentCorner);
                    }
                }

                if (hitRight.collider.tag == current.tag && hitDown.collider.tag == current.tag && hitRightDown.collider.tag == current.tag)
                {
                    GameObject split = Instantiate(splitter, new Vector2(current.transform.position.x + 0.5f, current.transform.position.y - 0.5f), Quaternion.identity);
                    split.GetComponent<SpriteRenderer>().color = current.GetComponent<SpriteRenderer>().color;
                    backObjects.Add(split);
                }
            }

        }
    }


    void CheckToWin()
    {

        if (GameObject.FindGameObjectsWithTag("White").Length != 0)
        {
            return;
        }


        foreach (var currentTag in tags)
        {
            GameObject[] darkblue = GameObject.FindGameObjectsWithTag(currentTag);
            int darkblueCount = -1;
            foreach (var obj in darkblue)
            {
                if (obj.transform.GetChild(0).GetChild(0).GetComponent<Text>().text != "")
                {
                    if (int.Parse(obj.transform.GetChild(0).GetChild(0).GetComponent<Text>().text) != darkblue.Length)
                    {
                        return;
                    }
                    darkblueCount = int.Parse(obj.transform.GetChild(0).GetChild(0).GetComponent<Text>().text);
                }
            }
            if (darkblueCount == 1)
            { 
                continue;
            }
            int oneSide = 0, twoSide = 0, threeSide = 0, fourSide = 0;

            foreach (var obj1 in darkblue)
            {
                bool have = false;
                int currentSide = 0;
                foreach (var obj2 in darkblue)
                {
                    if (obj1 == obj2) continue;

                    if (Math.Abs(obj1.transform.position.x - obj2.transform.position.x) == 1 && Math.Abs(obj1.transform.position.y - obj2.transform.position.y) == 0
                    || Math.Abs(obj1.transform.position.x - obj2.transform.position.x) == 0 && Math.Abs(obj1.transform.position.y - obj2.transform.position.y) == 1)
                    {
                        have = true;
                        currentSide++;

                        
                    }

                }
                
                if (currentSide == 1)
                    oneSide++;
                else if (currentSide == 2)
                    twoSide++;
                else if (currentSide == 3)
                    threeSide++;
                else if (currentSide == 4)
                    fourSide++;

                if (!have)
                {
                    print("NO");
                    return;
                }
            }

            if (darkblueCount == 4)
            { 
                if(twoSide >= 2 || threeSide == 1)
                {
                    
                }
                else
                {
                    print("NO 4");
                    return;
                }
            }
            if (darkblueCount == 5)
            {
                if (twoSide == 3 || twoSide == 1 && threeSide == 1 || fourSide == 1)
                {
                    
                }
                else
                {
                    print("NO 5");
                    return;
                }

            }
            if (darkblueCount == 6)
            {
                if (twoSide == 4 || twoSide == 4 && threeSide == 2 || fourSide == 1 && twoSide == 3)
                {

                }
                else if(threeSide == 2 && twoSide == 2 || threeSide == 1 && twoSide == 2)
                {

                }
                else
                {
                    print("NO 6");
                    return;
                }

            }

        }

        Win();
    }


    private void Win()
    {
        hintButton.GetComponent<Animation>().Play();
        nextButton.SetActive(true);
        nextButton.GetComponent<Animation>().Play();
        win = true;
        audioSource.PlayOneShot(winClip);

        for (int i = 0; i < colorCount; i++)
        {
            print("STARTTOCHECK");
            nestedLists.Add(new InnerList());

            nestedLists[i].elements = GameObject.FindGameObjectsWithTag(tags[i]).ToList();
            

        }

    }

    public void clickSound()
    {
        audioSource.PlayOneShot(clickClip);
    }

    public void next()
    {
        nextButton.GetComponent<Button>().interactable = false;
        PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level", 1) + 1);
        foreach (var item in blockText)
        {
            item.transform.parent.parent.GetComponent<Animation>().Play("blockAtEnd"); 
        }
        foreach (var item in backObjects)
        {
            item.transform.GetComponent<Animation>().Play("blockAtEnd");
        }
        Invoke("afterTimeNext", 0.5f);
    }
    public void hint()
    {
        //PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level", 1) + 1);
        //SceneManager.LoadScene("Level " + PlayerPrefs.GetInt("Level", 1));

        GameObject.Find("UnityAds").GetComponent<RewardedAd>().ShowAd();
    }

    private void afterTimeNext()
    {
        SceneManager.LoadScene("Level " + PlayerPrefs.GetInt("Level", 1));
        GameObject.Find("UnityAds").GetComponent<InterstitialAd>().ShowAd();
    }


    public void activateHint()
    {
        print("START");

        for (int i = 0; i < colorCount; i++)
        {
            GameObject[] current = GameObject.FindGameObjectsWithTag(tags[i]);
            if (nestedLists[i].elements.Count == current.Length)
            {
                bool doesntNeed = true;
                foreach (var item in current)
                {
                    if (!nestedLists[i].elements.Contains(item))
                    {
                        doesntNeed = false;
                    }
                }

                if (doesntNeed)
                {
                    continue;
                }
            }

            foreach (var item in current)
            {
                item.tag = "White";
                item.GetComponent<SpriteRenderer>().color = Color.white;
            }


            foreach (var item in nestedLists[i].elements)
            {
                item.tag = tags[i];
                item.GetComponent<SpriteRenderer>().color = colors[i];
            }

            CreateBorder();
            CheckToWin();
            return;
        }

            
    }
}
