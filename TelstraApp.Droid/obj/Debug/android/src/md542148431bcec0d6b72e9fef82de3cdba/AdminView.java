package md542148431bcec0d6b72e9fef82de3cdba;


public class AdminView
	extends mvvmcross.droid.views.MvxActivity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("TelstraApp.Droid.Views.AdminView, TelstraApp.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", AdminView.class, __md_methods);
	}


	public AdminView () throws java.lang.Throwable
	{
		super ();
		if (getClass () == AdminView.class)
			mono.android.TypeManager.Activate ("TelstraApp.Droid.Views.AdminView, TelstraApp.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
