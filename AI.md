# AI Components

* Override the player base as AI
    * give specific additional instructions to attach AI Processing scripts to pieces
    * hooked into when its turn starts
        * do the explore -> eval -> move loop
            * Call explore for all CommanderPieceBase
            * Call AIEval for all AIPieceCommander
            * Pick moves returned by commanders with the highest score

* AI Piece Component
    * AIPieceBase
        * AIPieceCommander
            * Call the soldiers AI of the soldiers this commander has 
                * reference the soldiers list in the CommanderPieceBase Component
            * Get best move of itself as well
            * return the best move of itself and all its soldiers Above a certain threshold (otherwise no move)
        * AiPieceSoldier
            * request all actions related to this piece from the database O(1)
            * Evaluate each action and get a score
            * Return the best move from all the actions its evaluated

# Fuzzy Logic Components
* Fuzzy Linguistic Variable
    * Generic with extensions
        * extensions hooks up the specific crisp input
        * return a fuzzy set
* Fuzzy Set
    * Operators
        * AND
        * OR
* Fuzzy Rule Set
    * Collection of Rules containing FLV
    * Asks the inputs to convert their crisp input to fuzzy input
    * performs and operators as neccecary 
    * sums the entire results
* Composite Set
    * Deffuzification

# Fuzzy Logic Variables

* Desirability
    * Output
    * Higher score -> Better move
* Risk
    * How likely are we to get taken?
* Reward
    * Piece advantage in capture