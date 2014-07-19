ChatScript
==========

ChatScript for Unity 4.x+ that I created. Basically drop this script onto any game object, something like a MainCamera or Player would be fine. Once you have the script on an object you need to set the lock and unlock textures on the script.

The window size for the chat box is defined at the top, you could adjust this to be player set, or have values in the editor and set the size in Start() as well.

There's a class for ChatEntry, which contains the things that you see in the chat, like time, name, and text. The OnGUI function will show the chat box if the showchat boolean is true, this allows you to hide and show the chat window at will.

So far the window works like most chat windows would, enter will focus it, pressing escape will focus out. You can lock and unlock the window with the lock icons in the top right. There's also a rudimentary slash command function setup so you could create special functions or things to run through chat commands in game.
