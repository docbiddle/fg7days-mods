using System;
using UnityEngine;

public class BlockCCTVScreen : Block
{
	////////////////////////////////////
	////		   CCTV v2			////
	////		  Alpha 16.2	    ////
	////////////////////////////////////
	
	public GameObject screenOBJ_0;
	public GameObject screenOBJ_1;
	public GameObject screenOBJ_2;
	public GameObject screenOBJ_3;
	
	public MeshRenderer screen0Mesh;
	public MeshRenderer screen1Mesh;
	public MeshRenderer screen2Mesh;
	public MeshRenderer screen3Mesh;
	
	public bool DebugOn = false;
	
	public static bool Screen0(byte _metadata)
	{
		return ((int)_metadata & 1 << 0) != 0;
	}
	public static bool Screen1(byte _metadata)
	{
		return ((int)_metadata & 1 << 1) != 0;
	}
	public static bool Screen2(byte _metadata)
	{
		return ((int)_metadata & 1 << 2) != 0;
	}
	public static bool Screen3(byte _metadata)
	{
		return ((int)_metadata & 1 << 3) != 0;
	}
	
	
	public override void OnBlockUnloaded(WorldBase _world, int _clrIdx, Vector3i _blockPos, BlockValue _blockValue)
	{
		BlockEntityData _ebcd = _world.ChunkClusters[_clrIdx].GetBlockEntity(_blockPos);
		if (!_blockValue.ischild)
		{
			_blockValue.meta = (byte) (_blockValue.meta & ~(1 << 0));
			_blockValue.meta = (byte) (_blockValue.meta & ~(1 << 1));
			_blockValue.meta = (byte) (_blockValue.meta & ~(1 << 2));
			_blockValue.meta = (byte) (_blockValue.meta & ~(1 << 3));
			_world.SetBlockRPC(_blockPos, _blockValue);
			
			_blockValue.meta = (byte) (_blockValue.meta | (0 << 0));
			_blockValue.meta = (byte) (_blockValue.meta | (0 << 1));
			_blockValue.meta = (byte) (_blockValue.meta | (0 << 2));
			_blockValue.meta = (byte) (_blockValue.meta | (0 << 3));
			_world.SetBlockRPC(_blockPos, _blockValue);
			this.shape.OnBlockUnloaded(_world, _clrIdx, _blockPos, _blockValue);
		}
	}
	
	public override void OnBlockLoaded(WorldBase _world, int _clrIdx, Vector3i _blockPos, BlockValue _blockValue)
	{
		BlockEntityData _ebcd = _world.ChunkClusters[_clrIdx].GetBlockEntity(_blockPos);
		if (!_blockValue.ischild)
		{
			this.shape.OnBlockLoaded(_world, _clrIdx, _blockPos, _blockValue);
			_blockValue.meta = (byte) (_blockValue.meta & ~(1 << 0));
			_blockValue.meta = (byte) (_blockValue.meta & ~(1 << 1));
			_blockValue.meta = (byte) (_blockValue.meta & ~(1 << 2));
			_blockValue.meta = (byte) (_blockValue.meta & ~(1 << 3));
			_world.SetBlockRPC(_blockPos, _blockValue);
			
			_blockValue.meta = (byte) (_blockValue.meta | (0 << 0));
			_blockValue.meta = (byte) (_blockValue.meta | (0 << 1));
			_blockValue.meta = (byte) (_blockValue.meta | (0 << 2));
			_blockValue.meta = (byte) (_blockValue.meta | (0 << 3));
			_world.SetBlockRPC(_blockPos, _blockValue);
		}
	}
	
	public override void OnBlockAdded(WorldBase _world, Chunk _chunk, Vector3i _blockPos, BlockValue _blockValue)
	{
		if (_blockValue.ischild)
		{
			return;
		}
		this.shape.OnBlockAdded(_world, _chunk, _blockPos, _blockValue);
		_blockValue.meta = (byte) (_blockValue.meta | (0 << 0));
		_blockValue.meta = (byte) (_blockValue.meta | (0 << 1));
		_blockValue.meta = (byte) (_blockValue.meta | (0 << 2));
		_blockValue.meta = (byte) (_blockValue.meta | (0 << 3));
		_world.SetBlockRPC(_blockPos, _blockValue);
		DisplayChatAreaText("Press Keypad 8 while looking at the screen for CCTV control instructions.");
		if (this.isMultiBlock)
		{
			this.multiBlockPos.AddChilds(_world, _chunk.ClrIdx, _blockPos, _blockValue);
		}
	}
	
	public override void OnBlockEntityTransformBeforeActivated(WorldBase _world, Vector3i _blockPos, int _cIdx, BlockValue _blockValue, BlockEntityData _ebcd)
    {
		ForceScreens(_blockPos, _blockValue, _ebcd);
		this.shape.OnBlockEntityTransformBeforeActivated(_world, _blockPos, _cIdx, _blockValue, _ebcd);
	}
	
	private BlockActivationCommand[] QJ = new BlockActivationCommand[1]
    {
        new BlockActivationCommand("Use", "hand", false)
    };
	
	public override string GetActivationText(WorldBase _world, BlockValue _blockValue, int _clrIdx, Vector3i _blockPos,
        EntityAlive _entityFocusing)
    {
		
		BlockEntityData _ebcd = _world.ChunkClusters[_clrIdx].GetBlockEntity(_blockPos);
		//GameObjects
		screenOBJ_0 = _ebcd.transform.FindChild("TV/TVScreen1").gameObject;
		screenOBJ_1 = _ebcd.transform.FindChild("TV/TVScreen2").gameObject;
		screenOBJ_2 = _ebcd.transform.FindChild("TV/TVScreen3").gameObject;
		screenOBJ_3 = _ebcd.transform.FindChild("TV/TVScreen4").gameObject;
		//MeshRenderers
		screen0Mesh = screenOBJ_0.GetComponent<MeshRenderer>();
		screen1Mesh = screenOBJ_1.GetComponent<MeshRenderer>();
		screen2Mesh = screenOBJ_2.GetComponent<MeshRenderer>();
		screen3Mesh = screenOBJ_3.GetComponent<MeshRenderer>();
		ForceScreens(_blockPos, _blockValue, _ebcd);
		if (Input.GetKeyUp(KeyCode.Keypad1))
		{
			screen0Mesh.enabled = true;
			screen1Mesh.enabled = false;
			screen2Mesh.enabled = false;
			screen3Mesh.enabled = false;
			_blockValue.meta = (byte) (_blockValue.meta | (1 << 0));
			_blockValue.meta = (byte) (_blockValue.meta & ~(1 << 1));
			_blockValue.meta = (byte) (_blockValue.meta & ~(1 << 2));
			_blockValue.meta = (byte) (_blockValue.meta & ~(1 << 3));
			_world.SetBlockRPC(_blockPos, _blockValue);
			return "";
		}
		
		
		if (Input.GetKeyUp(KeyCode.Keypad2))
		{
			screen0Mesh.enabled = false;
			screen1Mesh.enabled = true;
			screen2Mesh.enabled = false;
			screen3Mesh.enabled = false;
			_blockValue.meta = (byte) (_blockValue.meta & ~(1 << 0));
			_blockValue.meta = (byte) (_blockValue.meta | (1 << 1));
			_blockValue.meta = (byte) (_blockValue.meta & ~(1 << 2));
			_blockValue.meta = (byte) (_blockValue.meta & ~(1 << 3));
			_world.SetBlockRPC(_blockPos, _blockValue);
			return "";
		}
		
		
		if (Input.GetKeyUp(KeyCode.Keypad3))
		{
			screen0Mesh.enabled = false;
			screen1Mesh.enabled = false;
			screen2Mesh.enabled = true;
			screen3Mesh.enabled = false;
			_blockValue.meta = (byte) (_blockValue.meta & ~(1 << 0));
			_blockValue.meta = (byte) (_blockValue.meta & ~(1 << 1));
			_blockValue.meta = (byte) (_blockValue.meta | (1 << 2));
			_blockValue.meta = (byte) (_blockValue.meta & ~(1 << 3));
			_world.SetBlockRPC(_blockPos, _blockValue);
			return "";
		}
		
		
		if (Input.GetKeyUp(KeyCode.Keypad4))
		{
			screen0Mesh.enabled = false;
			screen1Mesh.enabled = false;
			screen2Mesh.enabled = false;
			screen3Mesh.enabled = true;
			_blockValue.meta = (byte) (_blockValue.meta & ~(1 << 0));
			_blockValue.meta = (byte) (_blockValue.meta & ~(1 << 1));
			_blockValue.meta = (byte) (_blockValue.meta & ~(1 << 2));
			_blockValue.meta = (byte) (_blockValue.meta | (1 << 3));
			_world.SetBlockRPC(_blockPos, _blockValue);
			return "";
		}
		if (Input.GetKeyUp(KeyCode.Keypad8))
		{
			DisplayChatAreaText("Turn the screen on by selecting a channel with numpad 1, 2, 3 or 4. You can pan the camera left and right using the arrow keys.");
			DisplayChatAreaText("Press E while looking at the screen to turn off the screen and all cams.");
			DisplayChatAreaText("Notes: In order to reduce frame rate loss only one camera is turned on at any time and all cameras are turned of when the screen is off.");
			DisplayChatAreaText("For this reason it's best to have only one screen turned on at a time.");
			return "";
		}
		if (!BlockCCTVScreen.Screen0(_blockValue.meta) && !BlockCCTVScreen.Screen1(_blockValue.meta) && !BlockCCTVScreen.Screen2(_blockValue.meta) && !BlockCCTVScreen.Screen3(_blockValue.meta))
		{
			return "To Turn On Select A Channel Using The NumPad.";
		}
		else
		return "";
	}
	
	
	
	
	public override bool OnBlockActivated(WorldBase _world, int _clrIdx, Vector3i _blockPos, BlockValue _blockValue, EntityAlive _player)
	{
		BlockEntityData _ebcd = _world.ChunkClusters[_clrIdx].GetBlockEntity(_blockPos);
		if (!BlockCCTVScreen.Screen0(_blockValue.meta) && !BlockCCTVScreen.Screen1(_blockValue.meta) && !BlockCCTVScreen.Screen2(_blockValue.meta) && !BlockCCTVScreen.Screen3(_blockValue.meta))
		{
			return false;
		}
		else
		{
			screen0Mesh.enabled = false;
			screen1Mesh.enabled = false;
			screen2Mesh.enabled = false;
			screen3Mesh.enabled = false;
			_blockValue.meta = (byte) (_blockValue.meta & ~(1 << 0));
			_blockValue.meta = (byte) (_blockValue.meta & ~(1 << 1));
			_blockValue.meta = (byte) (_blockValue.meta & ~(1 << 2));
			_blockValue.meta = (byte) (_blockValue.meta & ~(1 << 3));
			_world.SetBlockRPC(_blockPos, _blockValue);
			ForceScreens(_blockPos, _blockValue, _ebcd);
			return false;
		}
		
	}
	
	public void ForceScreens(Vector3i _blockPos, BlockValue _blockValue, BlockEntityData _ebcd)
	{
		//GameObjects
		screenOBJ_0 = _ebcd.transform.FindChild("TV/TVScreen1").gameObject;
		screenOBJ_1 = _ebcd.transform.FindChild("TV/TVScreen2").gameObject;
		screenOBJ_2 = _ebcd.transform.FindChild("TV/TVScreen3").gameObject;
		screenOBJ_3 = _ebcd.transform.FindChild("TV/TVScreen4").gameObject;
		//MeshRenderers
		screen0Mesh = screenOBJ_0.GetComponent<MeshRenderer>();
		screen1Mesh = screenOBJ_1.GetComponent<MeshRenderer>();
		screen2Mesh = screenOBJ_2.GetComponent<MeshRenderer>();
		screen3Mesh = screenOBJ_3.GetComponent<MeshRenderer>();
		
		//System Off
		if (!BlockCCTVScreen.Screen0(_blockValue.meta) && !BlockCCTVScreen.Screen1(_blockValue.meta) && !BlockCCTVScreen.Screen2(_blockValue.meta) && !BlockCCTVScreen.Screen3(_blockValue.meta))
		{
			screen0Mesh.enabled = false;
			screen1Mesh.enabled = false;
			screen2Mesh.enabled = false;
			screen3Mesh.enabled = false;
		}
		//Screen 0 on
		if (BlockCCTVScreen.Screen0(_blockValue.meta))
		{
			screen0Mesh.enabled = true;
			screen1Mesh.enabled = false;
			screen2Mesh.enabled = false;
			screen3Mesh.enabled = false;
			
		}
		//Screen 1 on
		if (BlockCCTVScreen.Screen1(_blockValue.meta))
		{
			screen0Mesh.enabled = false;
			screen1Mesh.enabled = true;
			screen2Mesh.enabled = false;
			screen3Mesh.enabled = false;
			
		}
		//Screen 2 on
		if (BlockCCTVScreen.Screen2(_blockValue.meta))
		{
			screen0Mesh.enabled = false;
			screen1Mesh.enabled = false;
			screen2Mesh.enabled = true;
			screen3Mesh.enabled = false;
			
		}
		//Screen 3 on
		if (BlockCCTVScreen.Screen3(_blockValue.meta))
		{
			screen0Mesh.enabled = false;
			screen1Mesh.enabled = false;
			screen2Mesh.enabled = false;
			screen3Mesh.enabled = true;
			
		}
	}
	
	private DateTime dteNextToolTipDisplayTime;
	
	private void DisplayToolTipText(string str)
    {
		EntityPlayerLocal entity = GameManager.Instance.World.GetLocalPlayer();
		if (DateTime.Now > dteNextToolTipDisplayTime)
        {
            GameManager.ShowTooltip(entity, str);
			dteNextToolTipDisplayTime = DateTime.Now.AddSeconds(0.6f);
        }
    }
	
	private static void DisplayChatAreaText(string str)
    {
		EntityAlive entity = default(EntityAlive);//Say as server.
		//Say as Local player//EntityAlive entity = GameManager.Instance.World.GetLocalPlayer();
        if (GameManager.Instance != null)
        {
            GameManager.Instance.GameMessage(EnumGameMessages.Chat, str, entity);
        }
    }
}