using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ChatScript : MonoBehaviour {
    public bool showChat = true;

    Vector2 scrollViewVector = Vector2.zero;
    string innerText = "Inside the scrollview";
    string chatString = "Enter text here";

    string lastString = "Enter text here";

    class ChatEntry
    {
        public DateTime timeStamp;
        public string name = "";
        public string text = "";

        public ChatEntry()
        {
            timeStamp = DateTime.Now;
            name = "Unknown";
            text = "Blank text";
        }

        public ChatEntry(DateTime aTime, string aName, string aText)
        {
            timeStamp = aTime;
            name = aName;
            text = aText;
        }
    }

    List<ChatEntry> chatEntry = new List<ChatEntry>();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}
	
    void OnGUI()
    {
        if (showChat)
        {
            Event e = Event.current;
            string tmpString = String.Empty;

            // Sets our textfield name
            GUI.SetNextControlName("chatinput");
            chatString = GUI.TextField(new Rect(20, Screen.height - 175, 400, 30), chatString, 50);

            //if (Input.GetKeyDown(KeyCode.Return) )
            if (chatString != String.Empty && chatString != "Enter text here")
                if (e.keyCode == KeyCode.Return)
                {
                    // Add an entry to the list
                    chatEntry.Add(new ChatEntry(DateTime.Now, "Strom:", chatString));
                    // Set our laststring to our chatstring, this way we don't say the same thing over and over
                    lastString = chatString;
                    chatString = String.Empty;
                }

            // We hit esc, unfocus chat window
            if (e.keyCode == KeyCode.Escape)
                GUI.FocusControl("");

            // We're not focused on the chat window, refocus it if we hit enter, and clear the chatstring/laststring
            if (GUI.GetNameOfFocusedControl() != "chatinput" && e.keyCode == KeyCode.Return)
            {
                chatString = "";
                lastString = chatString;
                GUI.FocusControl("chatinput");
            }

            // Build our string from our chat entries
            foreach (ChatEntry tmpChat in chatEntry)
            {
                tmpString += tmpChat.timeStamp + " " + tmpChat.name + " " + tmpChat.text + "\n";
            }

            innerText = tmpString;

            // Set our scrollview name
            GUI.SetNextControlName("chatview");
            scrollViewVector = GUI.BeginScrollView(new Rect(20, Screen.height - 375, 425, 200), scrollViewVector, new Rect(0, 0, 400, 1000));

            // Set our textarea to our huge chat string
            innerText = GUI.TextArea(new Rect(0, 0, 400, 1000), innerText);

            // End the ScrollView
            GUI.EndScrollView();
        }
    }

    void AddChatEntry(DateTime aTime, string aName, string aText)
    {
        ChatEntry tmp = new ChatEntry(aTime, aName, aText);
        chatEntry.Add(tmp);
    }
}
