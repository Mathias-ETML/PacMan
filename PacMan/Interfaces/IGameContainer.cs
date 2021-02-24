using PacManGame.Entities;
using PacManGame.GameView;
using PacManGame.Interfaces;
using PacManGame.Map;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacManGame.Interfaces
{
    /// <summary>
    /// IObjectContainer Interface
    /// </summary>
    public interface IMapContainer : IDisposable
    {
        GameMap Map { get; set; }

        GameForm GameForm { get; set; }

        Graphics GameFormPanelGraphics { get; set; }
    }

    /// <summary>
    /// IEntityContainer Interface
    /// </summary>
    public interface IEntityContainer : IDisposable
    {
        List<Ghost> Ghosts { get; set; }

        List<PacMan> PacMans { get; set; }
    }

    /// <summary>
    /// IGameContainer class
    /// </summary>
    public interface IGameContainer : IEntityContainer, IMapContainer, IEnumerable<Entity>
    {

    }
}
