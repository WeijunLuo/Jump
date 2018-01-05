using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

	[SerializeField] private float m_MovePower = 5; //驱动球移动的力量值  
	[SerializeField] private bool m_UseTorque = true; //驱动球的时候是否使用扭转力，即旋转力  
	[SerializeField] private float m_MaxAngularVelocity = 25; //球最大能旋转的速度  
	[SerializeField] private float m_JumpPower = 2; //球跳跃时所产生的力  
	private const float k_GroundRayLength = 1f; //射线确认球是否在地上的长度  
	private Rigidbody m_Rigidbody;  //球的刚体  
	// 初始化  
	private void Start()  
	{  
		m_Rigidbody = GetComponent<Rigidbody>();  
		// 设置最大的角速度，默认是7  
		GetComponent<Rigidbody>().maxAngularVelocity = m_MaxAngularVelocity;  
	}  
	// 移动，由具体的控制器调用  
	public void Move(Vector3 moveDirection, bool jump)  
	{  
		if (m_UseTorque)  
		{  
			// 如果是否旋转力，则在轴线方向添加力，即要向x轴前进，顺时针旋转z轴；向z轴前进，逆时针旋转x轴，最后乘以力的值  
			m_Rigidbody.AddTorque(new Vector3(moveDirection.z, 0, -moveDirection.x)*m_MovePower);  
		}  
		else  
		{  
			// 线性力的话就直接加就行  
			m_Rigidbody.AddForce(moveDirection*m_MovePower);  
		}  
		//如果在地上而且弹跳键被按下，则网上添加一个冲力。这边用了一条射线往下发射，判断是否在地上，就不能用Vector3.down吗，非要用Vector3.up，我愣了三秒为什么要往上发射  
		if (Physics.Raycast(transform.position, -Vector3.up, k_GroundRayLength) && jump)  
		{  
			m_Rigidbody.AddForce(Vector3.up*m_JumpPower, ForceMode.Impulse);  
		}  
	}  
}
