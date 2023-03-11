export interface FormController<TState, TActions, TComputed> {
    state: TState;
    actions: TActions;
    computed: TComputed;
};
