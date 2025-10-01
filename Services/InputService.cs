/*
 * @Author: Arian Sjöström Shaafi
 * B.Sc Computer Science & Mobile IT
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Formula1Retro.Services
{
    public class InputService
    {
        private readonly HashSet<Key> _pressedKeys = new();

        public void OnKeyDown(Key key)
        {
            if (!_pressedKeys.Contains(key))
                _pressedKeys.Add(key);
        }

        public void OnKeyUp(Key key)
        {
            if (_pressedKeys.Contains(key))
                _pressedKeys.Remove(key);
        }

        public bool IsKeyPressed(Key key)
        {
            return _pressedKeys.Contains(key);
        }
    }
}
