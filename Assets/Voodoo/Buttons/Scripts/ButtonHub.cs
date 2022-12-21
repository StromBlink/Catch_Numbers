using System;

namespace Voodoo.Visual.UI.Button
{
    public class ButtonHub : AbstractButtonHub<ButtonHubState> { }

    [Serializable]
    public class ButtonHubState : AbstractButtonHubState
    {
        public string name;
        public override string StateName => name;
    }
}
