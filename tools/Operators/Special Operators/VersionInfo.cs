class VersionInfo {
    public int Capacity { get; }
    public int ErrorDigits { get; }
    public int Group1Per { get; }
    public int Group1Blocks { get; }
    public int Group2Blocks { get; }
    public int Remainder { get; }
    public VersionInfo(int capacity, int errorDigits, int group1Per, int group1Blocks, int group2Blocks, int remainder) {
        Capacity = capacity;
        ErrorDigits = errorDigits;
        Group1Per = group1Per;
        Group1Blocks = group1Blocks;
        Group2Blocks = group2Blocks;
        Remainder = remainder;
    }
    public static VersionInfo GetVersion(int versionNumber) {
        return Infos[versionNumber - 1];
    }
    private static VersionInfo[] Infos = new VersionInfo[40] {
        new VersionInfo(11, 13, 13, 1, 0, 0),
        new VersionInfo(20, 22, 22, 1, 0, 7),
        new VersionInfo(32, 18, 17, 2, 0, 7),
        new VersionInfo(46, 26, 24, 2, 0, 7),
        new VersionInfo(60, 18, 15, 2, 2, 7),
        new VersionInfo(74, 24, 19, 4, 0, 7),
        new VersionInfo(86, 18, 14, 2, 4, 0),
        new VersionInfo(108, 22, 18, 4, 2, 0),
        new VersionInfo(130, 20, 16, 4, 4, 0),
        new VersionInfo(151, 24, 19, 6, 2, 0),
        new VersionInfo(177, 28, 22, 4, 4, 0),
        new VersionInfo(203, 26, 20, 4, 6, 0),
        new VersionInfo(241, 24, 20, 8, 4, 0),
        new VersionInfo(258, 20, 16, 11, 5, 3),
        new VersionInfo(292, 30, 24, 5, 7, 3),
        new VersionInfo(322, 24, 19, 15, 2, 3),
        new VersionInfo(364, 28, 22, 1, 15, 3),
        new VersionInfo(394, 28, 22, 17, 1, 3),
        new VersionInfo(442, 26, 21, 17, 4, 3),
        new VersionInfo(482, 30, 24, 15, 5, 3),
        new VersionInfo(509, 28, 22, 17, 6, 4),
        new VersionInfo(565, 30, 24, 7, 16, 4),
        new VersionInfo(611, 30, 24, 11, 14, 4),
        new VersionInfo(661, 30, 24, 11, 16, 4),
        new VersionInfo(715, 30, 24, 7, 22, 4),
        new VersionInfo(751, 28, 22, 28, 6, 4),
        new VersionInfo(805, 30, 23, 8, 26, 4),
        new VersionInfo(868, 30, 24, 4, 31, 3),
        new VersionInfo(908, 30, 23, 1, 37, 3),
        new VersionInfo(982, 30, 24, 15, 25, 3),
        new VersionInfo(1030, 30, 24, 42, 1, 3),
        new VersionInfo(1112, 30, 24, 10, 35, 3),
        new VersionInfo(1168, 30, 24, 29, 19, 3),
        new VersionInfo(1228, 30, 24, 44, 7, 3),
        new VersionInfo(1283, 30, 24, 39, 14, 0),
        new VersionInfo(1351, 30, 24, 46, 10, 0),
        new VersionInfo(1423, 30, 24, 49, 10, 0),
        new VersionInfo(1499, 30, 24, 48, 14, 0),
        new VersionInfo(1579, 30, 24, 43, 22, 0),
        new VersionInfo(1663, 30, 24, 34, 34, 0)
    };
}