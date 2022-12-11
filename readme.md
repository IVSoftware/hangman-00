Your [question](https://stackoverflow.com/q/74751267/5438626) is "how to let Form load recognize the name of the button in the event handler of 26 buttons". You also mention that when you click on buttons your objective is to change the states of `bool` values for your game. 

One easy way to do the _second_ thing is to use a `Checkbox` that _looks_ like a button by setting its `Appearance` property to `Button`. Whether you end up using buttons or checkboxes that look like buttons, one way to simplify your `Load` event handler is to iterate _all_ the controls in your `MainForm` as shown below. This example will show how to not only recognize the `Name` of the control, but also use that name to automatically generate the characters of the alphabet that you require for the game setup and along the way add an event to handle the button when the user clicks.

Here's a view of a `TableLayoutPanel` in the designer window that holds 26 checkboxes.

![designer](https://github.com/IVSoftware/hangman-00/blob/master/hangman-00/Screenshots/designer.png)

This method will visit each and every control on your `Form`, calling the `initCheckBox` method on each one.

    public MainForm() =>  InitializeComponent();
    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        IterateControlTree(null, initCheckBox, null);
	}
    internal delegate bool FxControlDlgt(Control control, Object args);
	// Iterate the control tree with 'some function' generically known as `fX`.
	internal bool IterateControlTree(Control control, FxControlDlgt fX, Object args)
	{
		if (control == null) control = this;
		if (!fX(control, args)) return false;
		foreach (Control child in control.Controls)
		{
			if (!IterateControlTree(child, fX, args))
			{
				return false;
			}
		}
		return true;
	}

The specific `fX` that we'll pass is going to set the text by looking at the control name and extracting the integer from it. We'll also assign a `Click` handler to each checkbox.

    private bool initCheckBox(Control control, object args)
	{
		if (control is CheckBox checkbox)
		{
			// One way to autogenerate the Text from the Name property.
			string sval = checkbox.Name.Replace("checkBox", string.Empty);
			if(int.TryParse(sval, out int offset))
            {
				char c = (char)('A' + (offset - 1));
				checkbox.Text = c.ToString();
            }
			checkbox.Click += onAnyClick;
		}
		return true;
    }

Here's what the `MainForm' looks like now whan the program is run:

![screenshot](https://github.com/IVSoftware/hangman-00/blob/master/hangman-00/Screenshots/screenshot.png)

What you can do is take this generic functionality of the `Click` handler and extend it to perform the functionality you need for your game. As a [Minimal Reproducible Example](https://stackoverflow.com/help/minimal-reproducible-example) of how one would go about doing this, let's implement it by displaying the new `bool` value on the main form title bar:

    private void onAnyClick(object sender, EventArgs e)
	{
		if (sender is CheckBox checkbox)
		{
			Text = $"{checkbox.Text} = {checkbox.Checked}";
		}
	}

After clicking on "button" #19, the title bar display as follows:

![title bar text](https://github.com/IVSoftware/hangman-00/blob/master/hangman-00/Screenshots/title-bar-text.png)




