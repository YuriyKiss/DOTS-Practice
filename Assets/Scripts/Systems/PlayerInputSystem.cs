using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

using Input = InputWrapper.Input;

public class PlayerInputSystem : SystemBase
{
    protected override void OnUpdate()
    {
        if (Input.TouchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
        
            Entities.ForEach((ref MoveData moveData) =>
            {
                if (touch.phase == TouchPhase.Moved)
                {
                    moveData.playerInput = Vector2.ClampMagnitude(touch.deltaPosition, 1f);
                }
                else
                {
                    moveData.playerInput = float2.zero;
                }
            }).Run();
        }
        else
        {
            Entities.ForEach((ref MoveData moveData) =>
            {
                moveData.playerInput = float2.zero;
            }).Run();
        }
    }
}
