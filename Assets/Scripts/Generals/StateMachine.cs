using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void State_Update();

public sealed class StateMachine
{

    // State map
    private Dictionary<object, State_Update> m_StateMap = null;

    // Runtime state info
    private object m_PrevState = null;
    private object m_CurrentState = null;
    private object m_NextState = null;

    private int m_NextStatePrior = 0;
    private bool m_IsStateChange = false;
    private bool m_IsFirstUpdate = true;

    public StateMachine()
    {
        m_StateMap = new Dictionary<object, State_Update>();
    }


    // Create/Modify

    /// <summary>
    /// Set initial state
    /// </summary>
    /// <param name="state"> initial state </param>
    public void SetInitialState(object state)
    {
        if(Debug.isDebugBuild && state == null)
        {
            Debug.LogError("Cannot initialize state with (null)");
        }

        // Set initial state
        m_CurrentState = state;

    }

    /// <summary>
    /// Add new state which calls update 
    /// </summary>
    /// <param name="state"> new state </param>
    /// <param name="update"> update function at state </param>
    public void AddState(object state, State_Update update)
    {
        if(Debug.isDebugBuild && (state == null || update == null))
        {
            var print_format = state.ToString() + ", " + update.ToString();
            Debug.LogError("Cannot AddState : (" + print_format + ")");
        }

        if(Debug.isDebugBuild && m_StateMap.ContainsKey(state))
        {
            var print_format = state.ToString();
            Debug.LogError("Cannot AddState : state(" + print_format + ") already exist");
        }

        // Add state
        m_StateMap.Add(state, update);
    }

    /// <summary>
    /// Modify state update function
    /// </summary>
    /// <param name="state"> state </param>
    /// <param name="update"> update function to be changed </param>
    public void ModifyState(object state, State_Update update)
    {
        if (Debug.isDebugBuild && (state == null || update == null))
        {
            var print_format = state.ToString() + ", " + update.ToString();
            Debug.LogError("Cannot ModifyState : (" + print_format + ")");
        }

        if (Debug.isDebugBuild && !m_StateMap.ContainsKey(state))
        {
            var print_format = state.ToString();
            Debug.LogError("Cannot ModifyState : state(" + print_format + ") not exist");
        }

        m_StateMap[state] = update;
    }


    // StateInfo

    /// <summary>
    /// Get Previous State
    /// </summary>
    /// <returns> previous state </returns>
    public object GetPrevState()
    {
        return m_PrevState;
    }

    /// <summary>
    /// Get Current State
    /// </summary>
    /// <returns> current state </returns>
    public object GetCurrentState()
    {
        return m_CurrentState;
    }

    /// <summary>
    /// Check is first frame after state change
    /// </summary>
    /// <returns> is current state is first frame </returns>
    public bool IsFirstUpdate()
    {
        return m_IsFirstUpdate;
    }

    // Runtime
    // State machine must be safe on modification.

    /// <summary>
    /// Call update function on current state
    /// </summary>
    public void Update()
    {
        m_StateMap[m_CurrentState]();
    }

    /// <summary>
    /// Determine final state of next frame
    /// </summary>
    public void LateUpdate()
    {
        m_IsFirstUpdate = false;
        if (m_IsStateChange)
        {
            m_PrevState = m_CurrentState;
            m_CurrentState = m_NextState;
            m_NextState = null;

            m_NextStatePrior = 0;
            m_IsFirstUpdate = true;
            m_IsStateChange = false;
        }
    }

    /// <summary>
    /// Change state of state machine.
    /// Prior with not lower will be choosed
    /// </summary>
    /// <param name="state"> next state </param>
    /// <param name="prior"> priority default : 0 </param>
    public void ChangeState(object state, int prior = 0)
    {
        if(Debug.isDebugBuild && state == null)
        {
            Debug.LogError("Cannot change state with (null)");
        }

        
        if(m_NextStatePrior > prior && m_IsStateChange)
        {
            // reject state change
            return;
        }

        m_NextStatePrior = prior;
        m_NextState = state;
        m_IsStateChange = true;
    }
    


}
