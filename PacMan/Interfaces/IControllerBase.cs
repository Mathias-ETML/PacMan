using PacManGame.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacManGame.Interfaces
{
    /// <summary>
    /// IUpdatableObjectGameController interface
    /// </summary>
    public interface IController
    {
        GameContainer GameContainer { get; set; }

        void OnStart();

        void OnUpdate();
    }
}
