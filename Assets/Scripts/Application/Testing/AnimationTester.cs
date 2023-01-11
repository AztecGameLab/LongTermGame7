﻿using System.Collections.Generic;
using UnityEngine;

namespace Application.Testing
{
    public class AnimationTester : MonoBehaviour
    {
        public FlipbookAnimationPlayer player;
        public List<FlipbookAnimationData> data;

        private int _index;
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _index = (_index + 1) % data.Count;
                player.CurrentAnimation = data[_index];
            }
        }
    }
}