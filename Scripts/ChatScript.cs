using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ChatScript : MonoBehaviour {

    // Window will start 5 over and at the bottom of the screen, minus the height of the window
    // this puts it in the bottom left corner by default
    Rect windowSize = new Rect(5, Screen.height - 255, 375, 250);
    // Can we currently move the window?
	bool windowLocked = false;
    
    // Our textures for the lock and unlock icons
	public Texture lockIcon;
    public Texture unlockIcon;
	
    // Display the chat window?
    public bool showChat = true;
    // Are we showing the time in the chat window?
    bool showChatTime = true;

    // Vector2 for the scroll view sizing
    Vector2 scrollViewVector = Vector2.zero;
    // Some initial text strings
    string innerText = "Inside the scrollview";
    string chatString = "Enter text here";
    string lastString = "Enter text here";

    class ChatEntry
    {
        // Three elements to a chat message: Time, Name, and Text
        public DateTime timeStamp;
        public string name = "";
        public string text = "";

        public ChatEntry()
        {
            timeStamp = DateTime.Now;
            name = "Unknown";
            text = "Blank text";
        }

        // TimeStamp, Name, Text
        public ChatEntry(DateTime aTime, string aName, string aText)
        {
            timeStamp = aTime;
            name = aName;
            text = aText;
        }

        // Name, Text
        public ChatEntry(string aName, string aText)
        {
            name = aName;
            text = aText;
        }
    }

    // New list for our chat entries
    List<ChatEntry> chatEntry = new List<ChatEntry>();

	// Use this for initialization
	void Start () {
        // This will cause the chat window to be focused at start up
        //GUI.FocusControl("chatinput");
	}
	
	// Update is called once per frame
	void Update () {

	}
	
    void OnGUI()
    {
        // Create our window
        windowSize = GUI.Window(0, windowSize, chatWindow, "Chat");
    }

    void chatWindow(int windowid)
    {
        // Show chat if its enabled
        if (showChat)
        {
            // Events for capturing mouse positions later for clicking lock/unlock
            Event e = Event.current;
            string tmpString = String.Empty;

            // Where our lock/unlock texture will be, based off the window size
            Rect lockPosition = new Rect(windowSize.width - 20, 2, 15, 15);
            // Draw the lock/unlock icon based on the bool if its locked
            GUI.DrawTexture(lockPosition, windowLocked ? lockIcon : unlockIcon);

            // Check if we clicked on the lock icon
            if (lockPosition.Contains(e.mousePosition))
            {
                // Event checking, if we left clicked AND it was a mouse AND it was mouse down, 
                // then we toggle the lock/unlock state
                if (e.button == 0 && e.isMouse && e.type == EventType.MouseDown)
                    windowLocked = !windowLocked;
            }

            // Sets our textfield name so we can focus it later 
            GUI.SetNextControlName("chatinput");
            chatString = GUI.TextField(new Rect(2, windowSize.height - 32, windowSize.width - 4, 30), chatString, 400);

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
            
            // Initial text string is not empty, and its not the first string then we do normal chat
            if (chatString != String.Empty && chatString != "Enter text here")
            {
                // We've pressed enter, time to do stuff!
                if (e.keyCode == KeyCode.Return)
                {
                    // Add an entry to the list
                    chatEntry.Add(new ChatEntry(DateTime.Now, "Strom:", chatString));
                    // Send our string to the parser to see if it has a / in front for a command
                    parseChatCommand(chatString);
                    // Set our laststring to our chatstring, this way we don't say the same thing over and over
                    lastString = chatString;
                    chatString = String.Empty;

                    //GUI.FocusControl("");
                }
            }

            // Build our string from our chat entries
            foreach (ChatEntry tmpChat in chatEntry)
            {
                if (showChatTime)
                    tmpString += tmpChat.timeStamp + " " + tmpChat.name + " " + tmpChat.text + "\n";
                else
                    tmpString += tmpChat.name + " " + tmpChat.text + "\n";
            }

            innerText = tmpString;

            // Set our scrollview name
            GUI.SetNextControlName("chatview");     // 2, windowSize.height - 32, windowSize.width - 4, 30
            scrollViewVector = GUI.BeginScrollView(new Rect(2, 18, windowSize.width - 10, 200), scrollViewVector, new Rect(0, 0, windowSize.width - 28, 500));

            // Set our textarea to our huge chat string
            innerText = GUI.TextArea(new Rect(0, 0, windowSize.width - 24, 500), innerText);

            // End the ScrollView
            GUI.EndScrollView();
        }

        // Allow window to be draggable if its not locked
        if (!windowLocked)
            GUI.DragWindow();
    }

    // Add an entry to our chatentry list
    void AddChatEntry(DateTime aTime, string aName, string aText)
    {
        ChatEntry tmpEntry = new ChatEntry(aTime, aName, aText);
        chatEntry.Add(tmpEntry);
    }
    
    // Check if our chat command had a slash in it "/"
    void parseChatCommand(string command)
    {
		if(command.Length > 0)
			if (command.Substring(0,1) == "/")	// Could also use command[0] or command.StartsWith("/")
				AddChatEntry(DateTime.Now, "Strom", "Slash Command: " + command.Substring(1, command.Length - 1));
    }
}
