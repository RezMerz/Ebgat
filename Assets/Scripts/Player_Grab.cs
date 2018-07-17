//using system.collections;
//using system.collections.generic;
//using unityengine;

//public class player_grab : monobehaviour {
//    public float grab_range_;

//    bool is_grabing_;
//    vector2 direction_;

//    keycode grab_key_;
	
//    // use this for initialization
//    void start ()
//    {
//        grab_key_ = getcomponent<player_control>().grab_;
//    }
	
//    // update is called once per frame
//    void update ()
//    {
//        grab();
		
//    }
//    void grab()
//    {
//        if (!is_grabing_ && input.getkey(grab_key_))
//        {
//            if (input.getkey(keycode.uparrow))
//            {
//                if (getcomponent<moveable>().move_up(grab_range_, true))
//                {
//                    is_grabing_ = true;
//                    getcomponent<player_move>().jump_off();
//                    getcomponent<gravity>().deactivition();

//                }
//            }
//            else
//            {
//                direction_ =getcomponent<player_move>().get_direction();
//                if(direction_ == vector2.right)
//                {
//                    if (getcomponent<moveable>().move_right(grab_range_, true))
//                    {
//                        is_grabing_ = true;
//                        getcomponent<player_move>().jump_off();
//                        getcomponent<gravity>().deactivition();
//                    }
//                }
//                else
//                {
//                    if (getcomponent<moveable>().move_left(grab_range_, true))
//                    {
//                        is_grabing_ = true;
//                        getcomponent<player_move>().jump_off();
//                        getcomponent<gravity>().deactivition();
//                    }
//                }

//            }
//        }
//        if(is_grabing_ && input.getkeyup(grab_key_))
//        {
//            is_grabing_ = false;
//            getcomponent<gravity>().activation();
//        }
//    }
//}
