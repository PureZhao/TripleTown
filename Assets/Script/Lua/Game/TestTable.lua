local Class = require("Core.Class")
local TestTableBase = require("Game.TestTableBase")


local TestTable = Class("TestTable")
extendClass(TestTable, TestTableBase)


function TestTable:__init()
    -- TestTableBase.__init(self)
    self.name = "nil"
    print("LuaBehaviour Test")
end

return TestTable