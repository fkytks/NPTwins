using System;
using System.Collections.Generic;
using System.Text;

using System.Collections;

using System.IO;
using System.Drawing;

using SdlDotNet.Core;
using SdlDotNet.Input;
using SdlDotNet.Graphics;
using SdlDotNet.Graphics.Sprites;
using SdlDotNet.Audio;

namespace NPTwins
{
    public class Manager
    {
        protected Game m_game;
        LinkedList<Actor> m_actors = new LinkedList<Actor>();

        public enum ACTOR
        {
            NONE = 0,
            MAN = 1,
            BLOCK = 2,
            POT = 3,
            OUTLET = 4,
            HOLL = 5,
            WOOD = 6,
            WALL=99
        }

        public void Init(Game game)
        {
            m_game = game;

            Reset();
        }

        public void Reset()
        {
            m_actors.Clear();
        }

        public Actor Add(Actor actor)
        {
            m_actors.AddLast(actor);
            return actor;
        }

        public void AddByParam(ACTOR eAct, int x, int y)
        {
            Actor act = null;
            switch (eAct)
            {
                case ACTOR.MAN:
                    act = new ActorPlayer();
                    break;
            }

            if (act != null)
            {
                act.Init(m_game);
                act.X = x;
                act.Y = y;
                act.kind = eAct;

                Add(act);
            }
        }


        public void ForEach( Action<Actor> action ) {
            foreach (Actor act in m_actors)
            {
                action(act);
            }
        }


        public void Action()
        {
            foreach (Actor act in m_actors)
            {
                act.Action();

            }


            LinkedListNode<Actor> node = m_actors.First;
            while (node != null)
            {
              Actor act = node.Value;
             
              if (act.m_fKill) 
              {
                LinkedListNode<Actor> removingNode = node;
                node = node.Next;
             
                m_actors.Remove(removingNode);
              }
              else
              {
                  node = node.Next;
              }
            }

        }

        public void Draw(Surface surface)
        {
            foreach (Actor act in m_actors)
            {
                act.Draw(surface);
            }
        }


        public virtual Actor FindHit(Actor act)
        {
            return FindHit(act.Pos, act.Size);
        }

        public Actor FindHit(Vector vPos, float fSize)
        {
            foreach (Actor act in m_actors)
            {
                if (act.IsHit(vPos,fSize))
                {
                    return act;
                }
            }
            return null ;
        }

        public int GetCount() { return m_actors.Count; }

        public void GetGameStat(out bool fDead , out int nAllPot , out int nFullPot )
        {
            fDead = false;
            nAllPot = 0;
            nFullPot = 0;

            foreach (Actor act in m_actors)
            {
                ACTOR eKind = act.kind;
                if (eKind == ACTOR.MAN)
                {
                    if (((ActorPlayer)act).IsDead()) fDead = true;
                }

            }
        }


      


    }
}
