# GDIM32-Final
## Check-In
### Allen Hu
I worked on the Interactable system and the player interaction logic in PlayerController.cs, especially the parts that detect nearby objects, highlight the current target, and let the player interact with E. I also adjusted the on-screen interaction prompt so it is easier to see and hides at the right times, like during dialogue or task selection.
For the Boss, I organized the flow so the player talks to the Boss first, then gets the task choice panel, and then moves into the next step of the task. I also fixed a repeated dialogue bug by updating DialogueManager.cs, especially in StartDialogue(), DisplayNextLine(), and the frame-based checks that stop the same dialogue from triggering twice.
Another thing I contributed was the task choice and task progression system. In TaskChoose.cs, I worked on methods like ShowChoicePanel(), SelectTask(), and StartPendingTask() so the player can only choose a task once and the panel behaves correctly. In TaskFlowManager.cs, I set up the state flow for accepting a task, going to drink coffee first, and only then officially starting work. I also kept the top-screen prompt so the player still knows what to do after picking a task.
Looking back, my proposal was helpful as a starting point, but it was not detailed enough for all the small logic problems that came up during development. Once I started building, I had to figure out a lot more state handling than I expected, especially for how dialogue, input, and UI overlap. My architecture changed a bit too, because I realized it was better to centralize task progression in TaskFlowManager instead of spreading it across multiple scripts.
### Team Member Name 2
Put your individual check-in Devlog here.
### Team Member Name 3
Put your individual check-in Devlog here.


## Final Submission
### Group Devlog
Put your group Devlog here.


### Team Member Name 1
Put your individual final Devlog here.
### Team Member Name 2
Put your individual final Devlog here.
### Team Member Name 3
Put your individual final Devlog here.

## Open-Source Assets

 - [Low-Poly Office Set #1 [+140 Models] [VNB]](https://assetstore.unity.com/packages/3d/props/low-poly-office-set-1-140-models-vnb-327126) - Office Sprites

 - [Human Basic Motions FREE](https://assetstore.unity.com/packages/3d/animations/human-basic-motions-free-154271) - Character Animation

 - [Businessmen Pack with Props v2.0](https://assetstore.unity.com/packages/3d/characters/humanoids/humans/businessmen-pack-with-props-v2-0-236156) - Player, Boss, Co-worker Sprites

 - [Background Music](https://pixabay.com/zh/music/upbeat-music-10468/?utm_source=chatgpt.com) - Background Music
