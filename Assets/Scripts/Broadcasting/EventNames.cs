using UnityEngine;
using System.Collections;

public class EventNames {
	
	public class KeyboardInput {
		public const string KEY_INPUTS = "KEY_INPUTS";
		public const string INTERACT_PRESS = "INTERACT_PRESS";
		public const string INTERACT_HOLD = "INTERACT_HOLD";
		public const string INTERACT_TOGGLE = "INTERACT_TOGGLE";
		public const string DETAIN_PRESS = "DETAIN_PRESS";
	}

	public class GamepadInput { 
		public const string RIGHT_STICK_INPUT = "RIGHT_STICK_INPUT";
	}

	public class MouseInput {
		public const string MOUSE_POS = "MOUSE_POS";
		public const string LEFT_CLICK_PRESS = "LEFT_CLICK_PRESS";
		public const string RIGHT_CLICK_PRESS = "RIGHT_CLICK_PRESS";
	}

	public class Prompt {
		public const string PROMPT_NAMES_ADD = "PROMPT_NAMES_ADD";
		public const string PROMPT_NAMES_DELETE = "PROMPT_NAMES_DELETE";
	}

	public class Active {
		public const string INTERACT_ENABLE = "INTERACT_ENABLE";
		public const string INTERACT_DISABLE = "INTERACT_DISABLE";
		public const string INTERACT_EXIT = "INTERACT_EXIT";
		public const string HIT_EXIT = "HIT_EXIT";
	}

	public class EnemySight{
		public const string PLAYER_SEEN = "PLAYER_SEEN";
	}
}







