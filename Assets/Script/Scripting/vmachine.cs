using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;
using System.Text.RegularExpressions;

public struct fact
{
    public int opcode;
    public int? operation;
    public float? param;
}
struct rule
{
    public List<fact> condition;
    public List<fact> action;
}

public class vmachine : MonoBehaviour {

    public Transform player;
    Transform boss;

    public const int INFRONT = 0;
    public const int BEHIND = 1;
    public const int ADVANCE = 2;
    public const int ROTATE = 3;
    public const int SHOOT = 4;
    public const int ANGLE = 5;
    public const int DISTANCE = 6;
    public const int DEFRULE = 7;
    public const int THEN = 8;
    public const int GREATER = 9;
    public const int SMALLER = 10;
    public const int EQUAL = 11;
    public const int FOLLOWING = 12;
    public const int NOTFOLLOWING = 13;
    public const int SPEED = 14;
    public const int WRONG_OPCODE = -1;

    readonly string[] defaultFile = { "defrule", "following", "=>", "speed 5.25", "defrule", "notfollowing", "=>", "speed 3.5" };

    int m_pc;                   // program counter
    int m_numrules;             // number of rules in the rule system
    rule[] m_program;             // the list of rules in the system

    //public TextAsset file;
    string fileName;
    string directory;

    // Use this for initialization
    void Start()
    {
        directory = "EnemyScripts/";
        fileName = "EnemyScripts/" + this.gameObject.name + ".txt"; 
        if(!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        if (!File.Exists(fileName))
        {            
            File.CreateText(fileName).Close();            
            File.WriteAllLines(fileName, defaultFile);
        }

        m_numrules = countRules(fileName);
        m_program = new rule[m_numrules];

        //load(file.name + ".txt");
        load(fileName);
        boss = this.gameObject.transform;
        
    }
    void Update()
    {
        run();
    }

    void load(string filename) // load rules from a script file
    {
        string line;
                
        StreamReader reader = new StreamReader(filename, Encoding.Default);
        
        line = reader.ReadLine();
        int i = 0;
        while (!reader.EndOfStream)
        {
            m_program[i].condition = new List<fact>();
            m_program[i].action = new List<fact>();
            line = reader.ReadLine();

            while (line != null && convertToOpcode(line) != THEN)
            {
                string[] words = line.Split(' ');
                fact f;
                // read a condition
                f.opcode = convertToOpcode(words[0]);
                switch (f.opcode)
                {
                    case ANGLE:
                        f.operation = convertToOpcode(words[1]);
                        f.param = float.Parse(words[2]);
                        m_program[i].condition.Add(f);
                        break;
                    case DISTANCE:
                        f.operation = convertToOpcode(words[1]);
                        f.param = float.Parse(words[2]);
                        m_program[i].condition.Add(f);
                        break;
                    case INFRONT:
                        f.operation = null;
                        f.param = null;
                        m_program[i].condition.Add(f);
                        break;
                    case BEHIND:
                        f.operation = null;
                        f.param = null;
                        m_program[i].condition.Add(f);
                        break;
                    case FOLLOWING:
                        f.operation = null;
                        f.param = null;
                        m_program[i].condition.Add(f);
                        break;
                    case NOTFOLLOWING:
                        f.operation = null;
                        f.param = null;
                        m_program[i].condition.Add(f);
                        break;
                }
                line = reader.ReadLine();
            }
            while(line != null && convertToOpcode(line) != DEFRULE)
            {
                string[] words = line.Split(' ');
                fact f;
                // read an action
                f.opcode = convertToOpcode(words[0]);
                switch(f.opcode)
                {
                    case ROTATE:
                        f.operation = null;
                        f.param = float.Parse(words[1]);
                        m_program[i].action.Add(f);
                        break;
                    case ADVANCE:
                        f.operation = null;
                        f.param = float.Parse(words[1]);
                        m_program[i].action.Add(f);
                        break;
                    case SHOOT:
                        f.operation = null;
                        f.param = null;
                        m_program[i].action.Add(f);
                        break;
                    case SPEED:
                        f.operation = null;
                        f.param = float.Parse(words[1]);
                        m_program[i].action.Add(f);
                        break;
                }
                line = reader.ReadLine();
            }
            i++;
        }

        reader.Close();

        if(m_program.Length == 0)
        {
            File.WriteAllLines(filename, defaultFile);
            Start();
        }
    }

    void run()           // execute the rule system
    {
        bool end = false;
        m_pc = 0;
        while ((!end) && (!valid(m_program[m_pc])))
        {
            m_pc++;
            if (m_pc >= m_program.Length)
            {
                end = true;
                m_pc--;
            }
        }

        if(valid(m_program[m_pc]))
        {
           run_action(m_program[m_pc]);
        }

    }

    bool valid(rule r)
    {
        foreach (var fact in r.condition)
        {
            if(fact.operation == WRONG_OPCODE || fact.opcode == WRONG_OPCODE)
            { return false; }
            switch (fact.opcode)
            {
                case ANGLE:
                    // determine angle to the player
                    float angle = Mathf.Acos(Vector3.Dot(player.transform.position.normalized, boss.transform.position.normalized));
                    if ((fact.operation == GREATER) && (angle < fact.param))
                    { return false; }
                    if ((fact.operation == SMALLER) && (angle > fact.param))
                    { return false; }
                    if ((fact.operation == EQUAL) && (angle != fact.param))
                    { return false; }
                    break;
                case DISTANCE:
                    float distance = Mathf.Sqrt(Mathf.Pow(player.position.x - boss.position.x, 2)+ Mathf.Pow(player.position.z - boss.transform.position.z, 2));
                    if((fact.operation == GREATER) && (distance < fact.param))
                    { return false; }
                    if ((fact.operation == SMALLER) && (distance > fact.param))
                    { return false; }
                    if ((fact.operation == EQUAL) && (distance != fact.param))
                    { return false; }
                    break;
                case INFRONT:
                    Vector3 IlocalPos = boss.InverseTransformPoint(player.position);
                    if(IlocalPos.x > 0.0)
                    { return false; }
                    break;
                case BEHIND:
                    Vector3 BlocalPos = boss.InverseTransformPoint(player.position);
                    if (BlocalPos.x < 0.0)
                    { return false; }
                    break;
                case FOLLOWING:
                    { return boss.GetComponent<Entity>().followingPlayer; }
                    break;
                case NOTFOLLOWING:
                    { return !boss.GetComponent<Entity>().followingPlayer; }
                    break;
            }
        };
        return false;
    }

    void run_action(rule r)
    {
        foreach(var fact in r.action)
        {
            switch(fact.opcode)
            {
                case ROTATE:
                    boss.RotateAround(boss.position, new Vector3(0, 1, 0), fact.param.Value);
                    break;
                case ADVANCE:
                    boss.transform.Translate(new Vector3(fact.param.Value * Mathf.Cos(boss.eulerAngles.y), 
                                                         0,
                                                         fact.param.Value * Mathf.Sin(boss.eulerAngles.y)));
                    break;
                case SPEED:
                    boss.GetComponent<Entity>().movementSpeed = fact.param.Value;
                    break;
            }
        }

    }	

    int countRules(string filename)
    {
        StreamReader reader = new StreamReader(filename, Encoding.Default);

        string file = reader.ReadToEnd();

        int countDefrule = System.Text.RegularExpressions.Regex.Matches(file.ToLower(), "defrule").Count;

        reader.Close();

        return countDefrule;

    }

    int convertToOpcode(string opcode)
    {
        if (opcode.ToLower() == "infront")
        { return INFRONT; }
        if (opcode.ToLower() == "behind")
        { return BEHIND; }
        if (opcode.ToLower() == "advance")
        { return ADVANCE; }
        if (opcode.ToLower() == "rotate")
        { return ROTATE; }
        if (opcode.ToLower() == "shoot")
        { return SHOOT; }
        if (opcode.ToLower() == "angle")
        { return ANGLE; }
        if (opcode.ToLower() == "distance")
        { return DISTANCE; }
        if (opcode.ToLower() == "defrule")
        { return DEFRULE; }
        if (opcode == "=>")
        { return THEN; }
        if (opcode == ">" || opcode.ToLower() == "greater")
        { return GREATER; }
        if (opcode == "<" || opcode.ToLower() == "smaller")
        { return SMALLER; }
        if (opcode == "=" || opcode.ToLower() == "equals")
        { return EQUAL; }
        if (opcode.ToLower() == "following")
        { return FOLLOWING; }
        if (opcode.ToLower() == "notfollowing")
        { return NOTFOLLOWING; }
        if (opcode.ToLower() == "speed")
        { return SPEED; }
        else return WRONG_OPCODE;
    }
}
