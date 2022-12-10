You mention several issues in your post so let me see if I can answer a couple of the main questions. Than, after giving this a tryt, consider posting new questions one-at-a-time for issues that remain. As I understand it, when you click on buttons your objective is to change the states of `bool` values for your game. The easy way to do that is to use a `Checkbox` that _looks_ like a button by setting its `Appearance` property to `Button`. Here's a view of a `TableLayoutPanel` in the designer window that holds 26 checkboxes.

![designer](https://github.com/IVSoftware/hangman-00/blob/master/hangman-00/Screenshots/designer.png)

Your `Load` event of your `MainForm` can be simplified by iterating the control tree. 

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
			checkbox.Text = checkbox.Name.Replace("checkBox", string.Empty);
			checkbox.Click += onAnyClick;
		}
		return true;
    }

Here's what the `MainForm' looks like now whan the program is run:

![screenshot](https://github.com/IVSoftware/hangman-00/blob/master/hangman-00/Screenshots/screenshot.png)

What you can do is take this generic functionality of the `Click` handler and extend it to perform the functionality you need for your game. As a [Minimal Reproducible Example](https://stackoverflow.com/help/minimal-reproducible-example) of how one would go about doing this, let's implement it by displaying the new `bool` value on the main form title bar:

![title bar text](https://github.com/IVSoftware/hangman-00/blob/master/hangman-00/Screenshots/title-bar-text.png)




