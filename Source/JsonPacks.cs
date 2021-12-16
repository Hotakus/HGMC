namespace HGMC.Source
{
  public class JsonPacks
  {
    
    public class OK_ERROR
    {
      public string Header  { get; set; }
      public string DataType  { get; set; }
      public string Data  { get; set; }
    }
    
    public class FrameHead
    {
      public class FHData {
        public string cf  { get; set; }
      }

      public string Header  { get; set; }
      public string DataType  { get; set; }
      public FHData Data { get; set; }

      public FrameHead()
      {
        Data = new FHData();
      }
    }
  }
}