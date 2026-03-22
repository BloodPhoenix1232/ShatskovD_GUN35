using Game.GameEngine.Ecs;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public sealed class MovingGroupAgent
{
    private const float STOPPING_DISTANCE = 1.0f;
    private const float OBSTACLE_AVOID_DISTANCE = 2.5f;
    private const float EQUALS_POINT_DISTANCE = 0.1f;
    private const float COMPLETE_RADIUS = 4.0f;
    private const float COMPLETE_RADIUS_COEF = 4 * Mathf.PI;
    private const float FORMATION_RADIUS = 3.0f;

    private readonly List<MoveAgent> movingAgents;
    private readonly List<MoveAgent> completeAgents;
    private readonly List<MoveAgent> cache;
    private readonly Vector3 destination;
    private readonly float completeRadius;

    private Dictionary<MoveAgent, Vector3> agentDestinations;

    public MovingGroupAgent(IEnumerable<MoveAgent> agents, Vector3 destination)
    {
        this.movingAgents = new List<MoveAgent>(agents);
        this.completeAgents = new List<MoveAgent>();
        this.cache = new List<MoveAgent>();
        this.agentDestinations = new Dictionary<MoveAgent, Vector3>();

        this.destination = destination;
        this.completeRadius = Mathf.Max(COMPLETE_RADIUS, this.movingAgents.Count / COMPLETE_RADIUS_COEF);

        this.DistributePositions();
    }

    public bool IsCompleted()
    {
        return this.movingAgents.Count <= 0;
    }

    public void Update()
    {
        this.cache.Clear();
        this.cache.AddRange(this.movingAgents);

        CorrectAgentPaths(this.cache);
        AvoidObstacles(this.cache);
        CheckDistanceToDestination(this.cache);
        AvoidAgentsCollision(this.cache);
        this.CompleteAgents();
    }

    public void RemoveAgents(IEnumerable<MoveAgent> agents)
    {
        foreach (var agent in agents)
        {
            this.completeAgents.Remove(agent);
            this.movingAgents.Remove(agent);
            this.agentDestinations.Remove(agent);
        }
    }

    private void DistributePositions()
    {
        int agentCount = this.movingAgents.Count;
        if (agentCount == 0) return;

        if (agentCount == 1)
        {
            this.agentDestinations[this.movingAgents[0]] = this.destination;
            return;
        }

        float angleStep = 360f / agentCount;
        float radius = Mathf.Min(FORMATION_RADIUS, Mathf.Sqrt(agentCount) * 1.5f);

        for (int i = 0; i < agentCount; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector3 offset = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;
            Vector3 targetPosition = this.destination + offset;

            if (NavMesh.SamplePosition(targetPosition, out var hit, 2.0f, NavMesh.AllAreas))
            {
                targetPosition = hit.position;
            }

            this.agentDestinations[this.movingAgents[i]] = targetPosition;

            this.movingAgents[i].MoveToPosition(targetPosition);
        }
    }

    private void CorrectAgentPaths(List<MoveAgent> agents)
    {
        foreach (var agent in agents)
        {
            if (agent.CanCorrectPath && !agent.IsLastPoint)
            {
                agent.CorrectPath();
            }
        }
    }

    private void AvoidObstacles(List<MoveAgent> agents)
    {
        foreach (var agent in agents)
        {
            if (!agent.IsObstacleAvoid)
            {
                if (agent.TryGetNextPosition(out Vector3 nextPosition))
                {
                    float distanceToNext = Vector3.Distance(agent.transform.position, nextPosition);

                    if (distanceToNext < OBSTACLE_AVOID_DISTANCE)
                    {
                        Vector3 direction = (nextPosition - agent.transform.position).normalized;
                        RaycastHit hit;

                        if (Physics.Raycast(agent.transform.position, direction, out hit, OBSTACLE_AVOID_DISTANCE, LayerMask.GetMask("Obstacle")))
                        {
                            agent.StartAvoidObstacle();
                        }
                    }
                }
            }
        }
    }

    private void AvoidAgentsCollision(List<MoveAgent> agents)
    {
        for (int i = 0; i < agents.Count; i++)
        {
            for (int j = i + 1; j < agents.Count; j++)
            {
                var agent1 = agents[i];
                var agent2 = agents[j];

                float distance = Vector3.Distance(agent1.transform.position, agent2.transform.position);
                float minDistance = 1.5f;

                if (distance < minDistance)
                {
                    Vector3 direction = (agent1.transform.position - agent2.transform.position).normalized;
                    float force = (minDistance - distance) * 0.5f;

                    Vector3 push1 = direction * force;
                    Vector3 push2 = -direction * force;

                    if (!agent1.IsObstacleAvoid)
                    {
                        agent1.transform.position += push1;
                    }

                    if (!agent2.IsObstacleAvoid)
                    {
                        agent2.transform.position += push2;
                    }
                }
            }
        }
    }

    private void CheckDistanceToDestination(List<MoveAgent> agents)
    {
        foreach (var agent in agents)
        {
            if (this.agentDestinations.TryGetValue(agent, out Vector3 agentDestination))
            {
                float distanceToDestination = Vector3.Distance(agent.transform.position, agentDestination);

                if (distanceToDestination <= this.completeRadius)
                {
                    agent.CompleteOnce();
                }
            }
            else
            {
                float distanceToDestination = Vector3.Distance(agent.transform.position, this.destination);
                if (distanceToDestination <= this.completeRadius)
                {
                    agent.CompleteOnce();
                }
            }
        }
    }

    private void CompleteAgents()
    {
        this.cache.Clear();
        this.cache.AddRange(this.movingAgents);

        for (int i = 0, count = this.cache.Count; i < count; i++)
        {
            var agent = this.cache[i];
            if (agent.IsCompleted)
            {
                this.movingAgents.Remove(agent);
                this.completeAgents.Add(agent);
            }
        }
    }
}