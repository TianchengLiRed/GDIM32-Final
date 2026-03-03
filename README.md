# GDIM32-Final
## Check-In

### Group Devlog
A core feature in our game is allowing the player to interact with nearby objects in a clear way. To support this, we use spatial checks in PlayerController.cs. The code uses Physics.OverlapSphere() to detect colliders within a certain range around the player, and then compares the squared distance from the player to each interactable object to determine which one is the closest. This matters because our game includes multiple interactable objects in the same space, and we only want one object to be highlighted and available for interaction at a time.
We also use raycasting as part of movement and player control. In PlayerController.cs, a downward Physics.Raycast() is used to check whether the player is grounded. This is important for making jump behavior work correctly, since the player should only be able to jump when touching the ground. Using a raycast here gives us a simple and reliable way to test that condition in 3D space.
Vector math is also used in our NPC interaction system. In scripts such as LookAtPlayerInteractable.cs, NPCs and the Boss calculate the direction toward the player by subtracting positions and then rotating to face that direction. This helps conversations feel more natural, since the characters visually respond to the player’s position instead of staying fixed in one direction.

### Allen Hu

##### Interactable system
I worked on the Interactable system and the player interaction logic in PlayerController.cs, especially the parts that detect nearby objects, highlight the current target, and let the player interact with E. I also adjusted the on-screen interaction prompt so it is easier to see and hides at the right times, like during dialogue or task selection.
For the Boss, I organized the flow so the player talks to the Boss first, then gets the task choice panel, and then moves into the next step of the task. I also fixed a repeated dialogue bug by updating DialogueManager.cs, especially in StartDialogue(), DisplayNextLine(), and the frame-based checks that stop the same dialogue from triggering twice.
##### Task choice and progression system
Another thing I contributed was the task choice and task progression system. In TaskChoose.cs, I worked on methods like ShowChoicePanel(), SelectTask(), and StartPendingTask() so the player can only choose a task once and the panel behaves correctly. In TaskFlowManager.cs, I set up the state flow for accepting a task, going to drink coffee first, and only then officially starting work. I also kept the top-screen prompt so the player still knows what to do after picking a task.
Looking back, my proposal was helpful as a starting point, but it was not detailed enough for all the small logic problems that came up during development. Once I started building, I had to figure out a lot more state handling than I expected, especially for how dialogue, input, and UI overlap. My architecture changed a bit too, because I realized it was better to centralize task progression in TaskFlowManager instead of spreading it across multiple scripts.

### Tiancheng Li

In current stage of game development, Im mainly responsible for the implementatiin of basic game structure and player basic interaction system.
##### Game Structure:
The game structure at this stage is mainly controlled by three classes:
DialogueManager class use to controll dialogue flow as a instance
private Queue<DialogueLine> _lineQueue = new Queue<DialogueLine>(); used to store dialogue information from ScriptableObject Dialogue Data and determine the order in which the dialogue is displayed.
I basically use event to connect different logic:
public event Action<DialogueLine> OnLineStarted;
public event Action OnDialogueEnded;
The DisplayNextLine() method is used to update the dialogue when the player clicks(Input.GetKeyDown(KeyCode.E). When a new dialogue begins OnLineStarted?.Invoke(nextLine); instruct the UIManager listener to update its text to the next line. The EndDialogue() method is used to end dialogue state. When dialogue ends OnDialogueEnded?.Invoke(); instruct the UIManager listener to close dialoguePanel.

##### UIManager, Dialogue Manager, TaskChoose

UIManager class use to controll all UI features as a instance

UIManager subscribed different event respond differently to different interactions
DialogueManager.Instance.OnLineStarted += ShowLine;
DialogueManager.Instance.OnDialogueEnded += HideUI;
TaskChoose.Instance.OnChoicePanelShown += HideTaskResultPanels;
TaskChoose.Instance.OnChooseLeft += ShowLeftPanel;
TaskChoose.Instance.OnChooseRight += ShowRightPanel;

Different methods are applied based on different events. when OnlineStarted?.Invoke(nextLine) called ShowLine(DialogueLine line) use TMP to Update UI information to current ScriptableObject nextline by nameText.text = line.name; contentText.text = line.content; OnDialogueEnded?.Invoke(); calls HideUI() to hide dialoguePanel, ShowLeftPanel() ShowrightPanel() HideTaskResultPanels();used to display different UIs in different events.

##### AudioManager
AudioManager class use to controll audio play in different situations in the game as a instance

public AudioClip clickSound; 
public AudioClip hoverSound; 
public AudioClip moveSound;
public AudioClip drinkSound;,etc.

The script stores a large number of aduioclips to play using different methods in corresponding situations. 

public void PlayClick()
    {
        if (clickSound != null)
            sourceSFX.PlayOneShot(clickSound);
    }
public void PlayerDrink();
public void PlayerMove();,etc.

I only need to use AudioManager.Instance.PlayClick(); in other scripts to play the corresponding sound.

##### interaction system
I also implement interaction system through 
In PlayerController class i create Move() to controll players movement InterRange() used to detect interactive items with a layermask of type Interactable within the player's interaction range. 
In CameraFollow class i use CameraChange() to  synchronize player's transform to camera to achieve first person view, and the camera's view is moved based on mouse movement.
Created interactble class as parent inheritance that has Oninteract() different subclasses react differently during player interaction.
SetHighlight() all objects turn yellow when the player gets close.

Break down provide a crucial role for me to clarify my thoughts by break whole game in to different part so that I can know how should I build the game, like I wrote different manager that manage different functions separately and connect them through events. Just like what I did create AudioManager DialogueManager and UIManager to controll different function and connected by events like Onlinestarted?.Invoke(); I think the proposal is not detail enough, even though its helpful to reminds me what task i need to finish, however, there are still many details we need to confirm during the work, such as whether the toilet door can be opened, or the player's interaction logic. I changed my architecture plan by adding DialogueManager, because i aware that dialogue function is complex enough to create separately. I think I will add more detail in function description to make teammate have a clear understand of each function, also i will use trello to clear divison of labor, avoid confilct in code and encourage add more comments. 

### Yaokun Wan    

At this stage of development, I was responsible for all of the art direction, character animation setup, and full scene construction inside Unity. Our team’s breakdown worked well because we clearly separated responsibilities that my teammates focusing heavily on the coding systems, and I handled the entire visual and environmental layer of the game.

##### Character & Animations
For character and animation work, I imported all player and NPC models into Unity and configured their rigs and Animator Controllers. For the player, I set up animation states including Idle, Walk, Run, and Talk, and connected them using Animator parameters to ensure smooth transitions during gameplay. For the Boss and co-worker, I configured Idle and Talk states. I also adjusted transition conditions (such as disabling “Has Exit Time” when necessary) to make animation switching more responsive. In addition, I wrote a script to rotate NPC GameObjects toward the player during interaction by updating their transform. rotation, which improves immersion during dialogue.

##### Environment
Beyond animation, I constructed the entire environment inside Unity. I sourced office and restroom assets compatible with the Built-in Render Pipeline and built the full playable layout, including walls, floors, ceilings, and furniture placement. At this stage, the environment is not just a blockout but a detailed and fully navigable map. I also designed and implemented multiple lighting setups in the scene. This includes floor lamps and desk lamps using Point Light, ceiling hanging lights using Spot Light, and restroom mirror lights using Point Light. I adjusted intensity, range, and placement to balance atmosphere and visibility. To support team workflow, I organized the Hierarchy by separating non-interactive props from interactable GameObjects. This made it easier for my teammates to quickly attach interaction scripts to objects like the printer or coffee machine without confusion.

Looking back at our Proposal and W7 breakdown, it was actually very helpful for me personally. Before having a clear breakdown, I sometimes didn’t know where to start when building the project in Unity. The breakdown gave me a clearer structure of what needed to be done first, such as character setup, animation states, and environment construction. It also helped me measure progress more clearly. Instead of feeling like I was just placing objects randomly, I could check our breakdown and see whether I had completed a specific part of the plan. Having that structure made the building process more organized and less overwhelming.

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
