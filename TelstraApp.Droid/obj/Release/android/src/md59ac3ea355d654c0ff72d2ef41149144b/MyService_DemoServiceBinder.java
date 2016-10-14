package md59ac3ea355d654c0ff72d2ef41149144b;


public class MyService_DemoServiceBinder
	extends android.os.Binder
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("TelstraApp.Droid.Services.MyService+DemoServiceBinder, TelstraApp.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", MyService_DemoServiceBinder.class, __md_methods);
	}


	public MyService_DemoServiceBinder () throws java.lang.Throwable
	{
		super ();
		if (getClass () == MyService_DemoServiceBinder.class)
			mono.android.TypeManager.Activate ("TelstraApp.Droid.Services.MyService+DemoServiceBinder, TelstraApp.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public MyService_DemoServiceBinder (md59ac3ea355d654c0ff72d2ef41149144b.MyService p0) throws java.lang.Throwable
	{
		super ();
		if (getClass () == MyService_DemoServiceBinder.class)
			mono.android.TypeManager.Activate ("TelstraApp.Droid.Services.MyService+DemoServiceBinder, TelstraApp.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "TelstraApp.Droid.Services.MyService, TelstraApp.Droid, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", this, new java.lang.Object[] { p0 });
	}

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
