import { SupportedLanguage } from "presentation/assets/lang";
import {
  createContext,
  useContext,
  PropsWithChildren,
  useMemo,
  useReducer,
} from "react";
import { languageContextReducer } from "./language.reducer";
import { LanguageAPI, LanguageState } from "./LanguageContextProvider.types";
import { first, isUndefined } from "lodash";

const LanguageDataContext = createContext<LanguageState>({} as LanguageState);
const LanguageAPIContext = createContext<LanguageAPI>({} as LanguageAPI);

const getBrowserLanguage = (): SupportedLanguage => {
  const browserLanguage = first(navigator.language.split("-"));
  if (isUndefined(browserLanguage)) {
    return SupportedLanguage.RO;
  }

  return browserLanguage as SupportedLanguage;
};

export const LanguageContextProvider = ({ children }: PropsWithChildren) => {
  const [state, dispatch] = useReducer(languageContextReducer, {
    selectedLanguage: SupportedLanguage.RO,
  });

  const api: LanguageAPI = useMemo(() => {
    const setRomanian = () => {
      dispatch({ type: "setRomanianLanguage" });
    };

    const setEnglish = () => {
      dispatch({ type: "setEnglishLanguage" });
    };

    return { setRomanian, setEnglish };
  }, []);

  return (
    <LanguageAPIContext.Provider value={api}>
      <LanguageDataContext.Provider value={state}>
        {children}
      </LanguageDataContext.Provider>
    </LanguageAPIContext.Provider>
  );
};

export const useLanguageState = () => useContext(LanguageDataContext);
export const useLanguageAPI = () => useContext(LanguageAPIContext);
