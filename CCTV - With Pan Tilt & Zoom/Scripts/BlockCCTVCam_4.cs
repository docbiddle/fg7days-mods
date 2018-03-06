using System;
using UnityEngine;

public class BlockCCTVCam_4 : Block
{
	////////////////////////////////////
	////		CCTV Camera v2.01	////
	////		  Alpha 14.7	    ////
	////////////////////////////////////
	
	public CCTVCam4 cctvCam4;
	public GameObject cameraControlScripOBJ;
	public bool hasScriptBeenAdded = false;
	public GameObject cameraOBJ;
	public float minFOV;
	public bool xmlLoaded = false;
	//XML Settings
	public bool camHasLight;
	public float panSpeed;
	public float tiltSpeed;
	public bool interfaceDisabled;
	public float maxPanLeft;
	public float maxPanRight;
	public float zoomSpeed;
	
	public void LoadScript3(BlockEntityData _ebcd, Vector3i _blockPos, BlockValue _blockValue)
	{
		cameraControlScripOBJ = _ebcd.transform.gameObject;
		cameraControlScripOBJ.AddComponent<CCTVCam4>();
		cctvCam4 = cameraControlScripOBJ.GetComponent<CCTVCam4>();
		if (!xmlLoaded)
		{
			if (this.Properties.Values.ContainsKey("ZoomLevel"))
			{
				float.TryParse(this.Properties.Values["ZoomLevel"], out minFOV);
				cctvCam4.ZoomLevelFromXML = minFOV;
			}
			if (this.Properties.Values.ContainsKey("HasLights"))
			{
				bool.TryParse(this.Properties.Values["HasLights"], out camHasLight);
				cctvCam4.canUseLights = camHasLight;
			}
			if (this.Properties.Values.ContainsKey("PanSpeed"))
			{
				float.TryParse(this.Properties.Values["PanSpeed"], out panSpeed);
				cctvCam4.TurnSpeed = panSpeed;
			}
			if (this.Properties.Values.ContainsKey("TiltSpeed"))
			{
				float.TryParse(this.Properties.Values["TiltSpeed"], out tiltSpeed);
				cctvCam4.TiltSpeed = tiltSpeed;
			}
			if (this.Properties.Values.ContainsKey("InterfaceDisabled"))
			{
				bool.TryParse(this.Properties.Values["InterfaceDisabled"], out interfaceDisabled);
				cctvCam4.isInterfaceDisabled = interfaceDisabled;
			}
			if (this.Properties.Values.ContainsKey("MaxPanLeft"))
			{
				float.TryParse(this.Properties.Values["MaxPanLeft"], out maxPanLeft);
				cctvCam4.StartAngle = maxPanLeft;
			}
			if (this.Properties.Values.ContainsKey("MaxPanRight"))
			{
				float.TryParse(this.Properties.Values["MaxPanRight"], out maxPanRight);
				cctvCam4.EndAngle = maxPanRight;
			}
			if (this.Properties.Values.ContainsKey("ZoomSpeed"))
			{
				float.TryParse(this.Properties.Values["ZoomSpeed"], out zoomSpeed);
				cctvCam4.zoomSpeedFromXML = zoomSpeed;
			}
			xmlLoaded = true;
			return;
		}
		return;
	}

	public override bool OnBlockActivated(WorldBase _world, int _clrIdx, Vector3i _blockPos, BlockValue _blockValue, EntityAlive _player)
	{
		return true;
	}
	
	public override void OnBlockEntityTransformBeforeActivated(WorldBase _world, Vector3i _blockPos, int _cIdx, BlockValue _blockValue, BlockEntityData _ebcd)
    {
		LoadScript3(_ebcd, _blockPos, _blockValue);
		this.shape.OnBlockEntityTransformBeforeActivated(_world, _blockPos, _cIdx, _blockValue, _ebcd);
	}
	
	public override void OnBlockAdded(WorldBase _world, Chunk _chunk, Vector3i _blockPos, BlockValue _blockValue)
	{
		if (_blockValue.ischild)
		{
			return;
		}
		this.shape.OnBlockAdded(_world, _chunk, _blockPos, _blockValue);
		_blockValue.meta = (byte) (_blockValue.meta | (0 << 0));
		_world.SetBlockRPC(_blockPos, _blockValue);
		xmlLoaded = false;
		if (this.isMultiBlock)
		{
			this.multiBlockPos.AddChilds(_world, _chunk.ClrIdx, _blockPos, _blockValue);
		}
	}
	
	public virtual void OnBlockLoaded(WorldBase _world, int _clrIdx, Vector3i _blockPos, BlockValue _blockValue)
	{
		BlockEntityData _ebcd = _world.ChunkClusters[_clrIdx].GetBlockEntity(_blockPos);
		if (!_blockValue.ischild)
		{
			this.shape.OnBlockLoaded(_world, _clrIdx, _blockPos, _blockValue);
		}
		xmlLoaded = false;
		LoadScript3(_ebcd, _blockPos, _blockValue);
	}
	
	private BlockActivationCommand[] QJ = new BlockActivationCommand[1]
    {
        new BlockActivationCommand("Use", "hand", false)
    };

	public override string GetActivationText(WorldBase _world, BlockValue _blockValue, int _clrIdx, Vector3i _blockPos, EntityAlive _entityFocusing)
    {
		return "Cam4";
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