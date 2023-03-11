export function is<T>(obj: any): obj is T {
    if (obj as T) {
        return true;
    }

    return false;
}
