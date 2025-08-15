using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class TargetWord : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI targetText;

    private readonly string[] fourLetterWordsEasy = new string[] { "AIDE", "BIDE", "CEDI", "DICE", "FADE", "GIBE", "HIDE", "IDEA", "ACID", "BADE",
    "CAFE", "DEAF", "EACH", "FACE", "GAFF", "HAIL", "IBEX", "ACED", "BEEF", "CAGE", "DEAD", "EASE", "FADE", "GAGE", "HALF", "ICED", "ABBE", "BAIL",
    "CEDE", "DEED", "EAVE", "FIFE", "GAIL", "HEED", "IDLE", "AGEE", "BASE", "CEDI", "DEEP", "EGAD", "FIEF", "GAIA", "HEAD", "IDEA", "AGOG", "BAKE",
    "CALF", "DICE", "EDGE", "FIFE", "GAGE", "HEAL", "IBEX", "AGED", "BALD", "CAME", "DIVE", "FAFF", "HEEL", "IDLE", "AGES", "BALE", "CANE", "DIME",
    "EASE", "FAIL", "GALE", "HEED", "IBIS", "ACID", "BAIT", "CARE", "DACE", "EASY", "FACE", "GAFF", "HIKE", "ICED", "ACHE", "BACK", "CARD", "DEAD",
    "EACH", "GAGE", "HIDE", "ICEE", "ABLE", "DAFT", "EATS", "FACT", "GAIL", "HALF", "ACES", "BALM", "JUMP", "PALE", "PIPE", "LIMP", "PAIL", "JAIL",
    "PEAK", "KIND", "PINE", "LEAP", "NAPE", "MILD", "PINK", "LAMP", "MAIL", "POND", "LOPE", "MOPE", "NAIL", "KELP", "LAID", "PAIN", "PACT", "DOPE",
    "LEAN", "MIND", "OPAL", "PAID", "PALM", "PANE", "PARK", "PART", "PEAL", "PILE", "PLAN", "PLAY", "PLEA", "PLUG", "POEM", "POKE", "POLE", "POLL",
    "POLO", "POOL", "POPE", "PORT", "POSE", "RAID", "RAIL", "RAIN", "RAMP", "RANK", "REAP", "RIPE", "ROPE", "SAIL", "SALE", "SAND", "SEAL", "SING",
    "SLAP", "SLIP", "SOAP", "SOIL", "SOLD", "SOLE", "SOLO", "SPAN", "SPIN", "SPIT", "SPOT", "BIKE", "DIKE", "HIKE", "LIKE", "MIKE", "PIKE", "SIKE", 
    "TIKE", "DEAL", "DIAL", "FAIL", "FIND", "FOIL", "GAIN", "GAPE", "GOLD", "HALO", "HEAL", "HEAP", "HELD", "HELP", "HOLD", "HOLE", "HOPE", "JOIN", 
    "KNOB", "KNOT", "LAND", "QUIP", "JURY", "JIVE", "QUOD", "JUNK", "YULE", "JEEP"};

    private readonly string[] fourLetterWordsMedium = new string[] { "JUMP", "PALE", "PIPE", "LIMP", "PAIL", "JAIL", "PEAK", "KIND", "PINE", "LEAP",
    "NAPE", "MILD", "PINK", "LAMP", "MAIL", "POND", "LOPE", "MOPE", "NAIL", "KELP", "LAID", "PAIN", "PACT", "DOPE", "LEAN", "MIND", "OPAL",
    "PAID", "PALM", "PANE", "PARK", "PART", "PEAL", "PILE", "PLAN", "PLAY", "PLEA", "PLUG", "POEM", "POKE", "POLE", "POLL", "POLO", "POOL", "POPE",
    "PORT", "POSE", "RAID", "RAIL", "RAIN", "RAMP", "RANK", "REAP", "RIPE", "ROPE", "SAIL", "SALE", "SAND", "SEAL", "SING", "SLAP", "SLIP", "SOAP",
    "SOIL", "SOLD", "SOLE", "SOLO", "SPAN", "SPIN", "SPIT", "SPOT", "BIKE", "DIKE", "HIKE", "LIKE", "MIKE", "PIKE", "SIKE", "TIKE", "DEAL", "DIAL",
    "FAIL", "FIND", "FOIL", "GAIN", "GAPE", "GOLD", "HALO", "HEAL", "HEAP", "HELD", "HELP", "HOLD", "HOLE", "HOPE", "JOIN", "KNOB", "KNOT", "LAND" };

    private readonly string[] fourLetterWordsHard = new string[] { "QUIZ", "JAZZ", "QUIP", "JUMP", "ZINC", "QUIZ", "ZONE", "ZEST", "QUIP", "JURY",
    "XRAY", "YARN", "ZANY", "QUIZ", "WAXY", "YAWK", "ZEAL", "QUIP", "JADE", "JOLT", "QUIZ", "ZIPS", "RUBY", "QUAD", "YOGA", "ZERO", "QUIP", "JIVE",
    "JOWL", "QUID", "ZOOM", "YANK", "QUAD", "ZEBU", "QUIP", "JAMB", "JUNK", "QUAY", "ZING", "YULE", "QUAD", "ZEAL", "QUIP", "JEEP", "JOKE", "QUOD",
    "ZEST", "YOGI", "QUAD", "ZEBU" };

    private readonly System.Random random = new();

    public GameManager gameManager;

    public static string selectedWord;


    void Start()
    {
        DisplayWord();
    }


    void DisplayWord()
    {
        if (GameManager.difficulty == 1)
        {
            selectedWord = fourLetterWordsEasy[random.Next(fourLetterWordsEasy.Length)];
        }
        else if (GameManager.difficulty == 2)
        {
            selectedWord = fourLetterWordsMedium[random.Next(fourLetterWordsMedium.Length)];
        }
        else if (GameManager.difficulty == 3)
        {
            selectedWord = fourLetterWordsHard[random.Next(fourLetterWordsHard.Length)];
        }
        else
        {
            //selectedWord = fourLetterWordsEasy[random.Next(fourLetterWordsEasy.Length)];
            selectedWord = "ABCA";
        }

        AssignWord();
    }


    void AssignWord()
    {
        targetText.text = selectedWord;

        gameManager._targetWord = selectedWord;
        
        gameManager.targetWordLetters.AddRange(selectedWord.Select(c => c.ToString()).ToArray());
        
        gameManager.letterCount = gameManager.targetWordLetters.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());
    }
}
