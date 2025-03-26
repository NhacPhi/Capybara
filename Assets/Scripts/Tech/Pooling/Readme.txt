Pooling System Work With Component And GameObject Need Inject With VContainer
Use Prefab Or Prefab Component For Pool 
Example: 
    GameObject: PoolManager.Instance.SpawnObject(Prefab, position, rotation);
    Component: PoolManager.Instance.SpawnObject<YouComponentType>(Component, position, rotation);
    
GenericPool<T> Pool For Class 
When You = new YourClass(); Unity allocates memory on the heap, which can lead to Garbage Collection (GC) later.
If you use a struct, it is allocated on the stack or inlined in memory if it’s a struct inside a class.
If You New Class Foreach Frame Not Good For Performance 

Example: 
    StringBuilder sb = GenericPool<StringBuilder>.Get().Clear();
    sb.Append('t');
    textMesh.text = sb.ToString();
    GenericPool<StringBuilder>.Return(sb);

