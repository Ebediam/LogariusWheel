using BS;

namespace LogariusWheel
{
    // This create an item module that can be referenced in the item JSON
    public class ItemModuleLogariusWheel : ItemModule
    {
        public float switchSpeed = 10f;
        public override void OnItemLoaded(Item item)
        {
            base.OnItemLoaded(item);
            item.gameObject.AddComponent<ItemLogariusWheel>();
        }
    }
}
