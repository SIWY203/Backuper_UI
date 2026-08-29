class PathHelper
{
    public static bool IsSubdirectory(string src, string dest)
    {
        char[] separators = [Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar];
        string fullSrc = Path.GetFullPath(src).TrimEnd(separators);
        string fullDest = Path.GetFullPath(dest).TrimEnd(separators);

        string relative = Path.GetRelativePath(fullSrc, fullDest);

        return !relative.StartsWith("..") && relative != ".";
    }

}
