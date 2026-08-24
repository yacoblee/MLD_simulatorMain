namespace camProject.Model
{
    // 이식 대상: copyApp/Comm/ConfigFile.cs — 레시피 SV 값 JSON 저장/로드
    //
    // 필요한 멤버:
    //   - public int[] SelectedValue { get; set; } = Array.Empty<int>();
    //   - public bool Save(string path)
    //   - public static ConfigFile Load(string path)
    //
    // 포팅 시 주의:
    //   - Newtonsoft.Json 필요 → camProject.csproj에 PackageReference 추가되어 있음
    public class ConfigFile
    {
    }
}
