using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Condicion que devuelve un valor booleano dado que alterna entre verdadero y falso en funcíon del tiempo transcurrdio
 */
public class ConditionRandomOverTime : ICondition
{
    public bool isTrue = true;
    public float time = 8;
    public float timeVariance = 2;
    private float currentTime = 0;
    private float timer;
	private bool isCounting = false;

    private void Start()
    {
        StartCounting();
    }

    void Update()
    {
        if (isCounting)
        {
            timer += Time.deltaTime;
            if(timer >= currentTime)
            {
   
                isTrue = !isTrue;
                timer = 0;
                
            }
        }
    }

	public override bool Test()
	{
        return isTrue;
	}


	private void StartCounting()
    {
        timer = 0;
        currentTime = time + Random.Range(-timeVariance, timeVariance);
        isCounting = true;
    }

}
