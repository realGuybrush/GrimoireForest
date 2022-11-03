using System.Collections.Generic;

namespace MyAssets.Scripts {

    public static class VASUtilities<T> {
        public static void SetDefaultValuesSquareList(List<List<T>> list, int Width, int Height, T value) {
            for (int i = 0; i < Height; i++) {
                list.Add(new List<T>());
                for (int j = 0; j < Width; j++) {
                    list[i].Add(value);
                }
            }
        }
    }

}
