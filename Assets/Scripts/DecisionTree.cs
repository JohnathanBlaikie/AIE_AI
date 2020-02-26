using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionTree : MonoBehaviour
{

    public Transform home;
    public Transform goal;
    public int moneyFarmed;
    public int moneyCap;
    public float speed;
    // Start is called before the first frame update
    IDecision decisionTree;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
    public class MoneyCheck : IDecision
    {
        GameObject agent;
        IDecision tBranch, fBranch;
        int money;
        int tMoneyCap;

        public MoneyCheck() { }
        public MoneyCheck(GameObject a, IDecision tB, IDecision fB)
        {
            this.agent = a;
            this.tBranch = tB;
            this.fBranch = fB;
        }

        public IDecision Decide()
        {
            money = agent.GetComponent<DecisionTree>().moneyFarmed;
            tMoneyCap = agent.GetComponent<DecisionTree>().moneyCap;
            return money >= tMoneyCap ? tBranch : fBranch;
        }
    }

public class FindLocation : IDecision
{
    GameObject agent;
    Transform target;
    IDecision tBranch;
    IDecision fBranch;
    public FindLocation() { }

    public FindLocation(GameObject a, Transform t, IDecision tB, IDecision fB)
    {
        agent = a;
        target = t;
        tBranch = tB;
        fBranch = fB;
    }

    public IDecision Decide()
    {
        return Vector3.Distance(agent.transform.position, target.transform.position) < 1 ? tBranch : fBranch;
    }
}

