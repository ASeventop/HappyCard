using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UniRx;
public class TestLinq : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        List<int> nums1 = new List<int> { 1, 2, 1, 4, 3 };
        List<int> nums2 = new List<int> { 1, 4, 5, 7, 9 };

        List<string> actor1 = new List<string> {"0","2"};
        List<ActorTest> actor2 = new List<ActorTest> { new ActorTest { ID = "1", uid = 1 }
                                                        ,new ActorTest { ID = "0", uid = 0 }
                                                        ,new ActorTest { ID = "2", uid = 1 }
                                                     };
        //Dictionary<int, int> seats = new Dictionary<int, int> { { 1, 1 }, { 2, 3 }, { 3, 2 } };
       // Dictionary<int, bool> ready = new Dictionary<int, bool> { { 3, false }, { 2, true }, { 3, true } };
        Dictionary<string, bool> test1 = new Dictionary<string, bool>();
        Dictionary<string, int> test2 = new Dictionary<string, int>();



       /* var a = seats.Where(x => ready.ContainsKey(x.Key) && ready[x.Key] == true);
        var b = seats.Cast<DictionaryEntry>().ToDictionary(d => d.Key, d => d.Value);*/
       /* test1.Add("a", true);
        test1.Add("b", true);
        test1.Add("c", true);
        test1.Add("d", true);
        test1.Add("e", true);
        test1.Add("f", true);
        var test = test1.All(key => key.Value == true);
        Debug.Log("test " + test);*/
       /* var exists = test1.Keys.Any(x => x.Contains)

        Debug.Log();*/

        //var filterlist = actor2.Where(l => !actor1.Contains(l.ID)).ToList();
        //var aaa = actor2.Where(e => actor1.Contains));
       // Debug.Log("filterlist " + filterlist.Count);
       //Debug.Log(aaa.Count());

        ///////test
        /* Debug.Log(nums1.Any(x =>x == 1));
         Debug.Log(nums2.Any());*/
        //any ทั้งหมด
        // except ยกเว้น ถ้า list ที่เชคมีค่าเหมือนกัน เอาตัวนั้นของ list ตัวแรกออก
        // Debug.Log(nums1.Except(aaa => nums2.Any(bbb => bbb == aaa)));
        /* Debug.Log("exept num2 count"+nums1.Except(nums2).Count());
         nums1.Except(nums2).ToList().ForEach(item => {
             Debug.Log("item " + item);
         });
         if (nums1.Any(x => nums2.Any(y => y == x)))
         {
             Debug.Log("There are equal elements");
         }
         else
         {
             Debug.Log("No Match Found!");
         }*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
public class ActorTest
{
    public string ID;
    public byte uid;
}
